(function (document) {
	'use strict';
		
	if (!navigator.getUserMedia) {
		navigator.getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia || 
			navigator.msGetUserMedia;
	}
	
	if (!window.URL) {
		window.URL =  window.webkitURL || window.msURL || window.oURL;
	}
	
	Date.prototype.toDateInputValue = function () {
		var local = new Date(this);
		local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
		return local.toJSON().slice(0, 10);
	};
	  	  
	Date.prototype.toTimeInputValue = function () {
		var local = new Date(this);
		return local.toTimeString().slice(0, 8);
	};
	
	Date.prototype.toMsFormat = function () {
		var local = new Date(this);
		return '\/Date('+ local.getTime()+')\/';
	};
	
	Date.prototype.toFilename = function () {
		return this.toDateInputValue().replace(/-/g, '') +
			'_' + this.toTimeInputValue().replace(/:/g, '');
	};
	
	if (typeof String.prototype.startsWith !== 'function') {
		String.prototype.startsWith = function (str){
			return this.slice(0, str.length) === str;
		};
	}
	if (typeof String.prototype.endsWith !== 'function') {
		String.prototype.endsWith = function (str){
			return this.slice(-str.length) === str;
		};
	}
		
	Object.clone = function (src) {
		var newObj = (src instanceof Array) ? [] : {};
		if ((src === null || src instanceof Date || !(src instanceof Array ||  typeof (src) === 'object'))) {
			return src;
		}
		for (var i in src) {
			if (src[i] && typeof src[i] === 'object') {
				newObj[i] = Object.clone(src[i]);
			}
			else
			{
				newObj[i] = src[i];
			}
		}
		return newObj; 
	};
	
	Object.compare = function (o1, o2) {
		
		if(!o1){
			return !o2;
		}
		
		if (o1===null) {
			return o2===null?true:false;
		} 
		
		if ( o1 instanceof Date ) {
			return (o2 instanceof Date)? o1.toString()===o2.toString():false;
		}
		
		if (!(o1 instanceof Array || typeof (o1) === 'object')) {
			return  (!(o1 instanceof Array || typeof (o1) === 'object'))? o1===o2:false;
		}
			
		if (Object.getOwnPropertyNames(o1).length!==Object.getOwnPropertyNames(o2||{}).length) {
			return false;
		}
		for( var i in  o1){
			if ( typeof o1[i] === 'object') {
				if (!Object.compare(o1[i], o2[i])) {
					return false;
				}			
			}
			else{
				if(o1[i]!== o2[i]) {
					return false;
				}
			}
		}
		return true;
	};
			
	window.liebre={};
	window.liebre.tools={};
	window.liebre.remote={};
	
	window.liebre.IndexStorage = (function () {
		function IndexStorage(name, schema, options) {
			this.__name=name||'default-storage';
			this.__schema=schema||{};
			this.__options= options||{};
			this.__db=null;
			this.__ready=false;
		}
		Object.defineProperty(IndexStorage.prototype, 'name', {
			get: function () {
				return this.__name;
			},
			enumerable: true,
			configurable: true
		});
		Object.defineProperty(IndexStorage.prototype, 'opened', {
			get: function () {
				return this.__db!==null;
			},
			enumerable: true,
			configurable: true
		});
		IndexStorage.prototype.open = function () {
			var self=this;
			if(self.__db===null){
				self.__db = new window.ydn.db.Storage(self.__name, self.__schema, self.__options);
				self.__db.onReady(function(e) {
					if (e) {
						self.__db=null;
						self.__ready=false;
						if (e.target.error) {
							console.log('Error due to: ' + e.target.error.name + ' ' + e.target.error.message);
						}
						throw e;
					}
					self.__ready=true;
				});        
			}
		};
		IndexStorage.prototype.execute = function (fn) {
			var self=this;
			if(self.__ready){
				fn(self.__db);
				return;
			}
			(function(){
				var times=10;
				var tId = setInterval(function() {
					if (self.__ready) {
						onReady();
					}
					else{
						times--;
						if(times<=0) {
							clearInterval(tId);
						}
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					fn(self.__db);
				}
			})();
		};
		IndexStorage.prototype.close = function () {
			if(this.__db!==null){
				this.__db.close();
			}
		};
		return IndexStorage;
	})();
		
	
	window.liebre.tools.getBaseUrl=function(){
		return localStorage.getItem('liebre.baseUrl') ||
			( (location.protocol + '//' + location.host+ '/lbr-api').replace('9000','8080'));
	};
	
	window.liebre.tools.setBaseUrl=function(url){
		localStorage.setItem('liebre.baseUrl', url ||
							 ( (location.protocol + '//' + location.host+ '/lbr-api').replace('9000','8080')));
	};
		
	window.liebre.tools.toFormData=function(obj){
		if (!obj || obj === null ){
			return  null;
		}
				
		if ( obj instanceof Date ) {
			return  '"'+ obj.toISOString()+'"';
		}
		
		if (!(obj instanceof Array || typeof (obj) === 'object')) {
			return  (typeof obj === 'string')? '"'+obj+'"': obj;
		}
		
		if(Object.getOwnPropertyNames(obj).length===0){
			return '{}';
		}
		var r= (obj instanceof Array)?'[':'{';
		for(var p in obj){	
			if(obj[p] instanceof Date){
				r=r+(obj instanceof Array?'"':  p+':"')+obj[p].toISOString()+'",';
			}
			else{
				if(obj[p] instanceof Array){
					var ra ='[';
					for (var a in obj[p]){
						ra=ra+ window.liebre.tools.toFormData(obj[p][a])+',';
					}
					r= r+(obj instanceof Array?'':p+':')+
						(obj[p].length>0? ra.replace(/,([^,]*)$/,']'+'$1')+',': '[],');
				}
				else{
					if( typeof obj[p] === 'object'){
						r=r+(obj instanceof Array?'': p+':')+window.liebre.tools.toFormData(obj[p])+',';
					}
					else{
						var c = (typeof obj[p] === 'string')?'"':'';
						r=r+(obj instanceof Array?c:  p+':'+c)+obj[p]+c+',';
					}
				}
			}
		}
		return r.replace(/,([^,]*)$/, (obj instanceof Array?']':'}')+'$1');
	};
	
	window.liebre.tools.convertToJsDate= function (v){
		if (!v) {
			return null;
		}
		if (typeof v !== 'string'){
			return v;
		}
		return new Date(parseFloat(/Date\(([^)]+)\)/.exec(v)[1])); // thanks demis bellot!
		
	};
	
	window.liebre.tools.formatDate= function(value) {
		if(!value) {
			return '';
		}
		var v = window.liebre.tools.convertToJsDate(value);
		return v.toDateInputValue();
	};
	
	window.liebre.tools.blink=function(target, config){
		config=config||{};
	
		target.style.backgroundColor= config.backgroudColor||'white';
			target.style.color= config.backgroudColor|| 'rgb(8, 13, 49)';
			setTimeout(function(){
				target.style.backgroundColor='';
				target.style.color='';
			}, config.wait || 100);
	};
	
	window.liebre.remote.save=function(data, config){
		
		if(data.Id){
			window.liebre.remote.update(data, config);
		}
		else{
			window.liebre.remote.create(data, config);
		}
	};
			
	window.liebre.remote.read=function(config){
		config = config || {};
		config.response= config.response|| function(e){console.log(e);};
		config.error= config.error|| function(e){console.log(e);};
		config.errorConnection= config.errorConnection|| function(e){console.log(e);};
		config.complete= config.complete|| function(e){console.log(e);};
								
		var ajax= document.querySelector( config.selector|| '#read' );
		ajax.action= config.action || ajax.action;
		ajax.model=  config.model  || ajax.model;
		ajax.urlparams= config.urlparams || ajax.urlparams||{};
		ajax.urlparams.format = ajax.urlparams.format || 'json';
			
		ajax.errorConnectionHandler= config.errorConnection;
		ajax.errorHandler= config.error;
		ajax.responseHandler= config.response;
		ajax.completeHandler= config.complete;
		
		ajax.baseUrl= config.baseUrl || ajax.baseUrl || window.liebre.tools.getBaseUrl();
		ajax.go();
	};
		
	window.liebre.remote.create=function(data, config){
		config = config || {};	
		config.response= config.response|| function(e){console.log(e);};
		config.error= config.error|| function(e){console.log(e);};
		config.errorConnection= config.errorConnection|| function(e){console.log(e);};
		config.complete= config.complete|| function(e){console.log(e);};
								
		var ajax= document.querySelector( config.selector|| '#create' );
		ajax.action= config.action || ajax.action;
		ajax.model=  config.model  || ajax.model;
        ajax.headers = config.headers ||  '{"Content-Type":"application/x-www-form-urlencoded"}';
				
		ajax.params= JSON.stringify({Data: window.liebre.tools.toFormData(data) } );
						
		ajax.errorConnectionHandler= config.errorConnection;
		ajax.errorHandler= config.error;
		ajax.responseHandler= config.response;
		ajax.completeHandler= config.complete;
		
		ajax.baseUrl= config.baseUrl || ajax.baseUrl || window.liebre.tools.getBaseUrl();	
		ajax.go();
	};
	
	window.liebre.remote.update=function(data, config){
		config = config || {};	
		config.response= config.response|| function(e){console.log(e);};
		config.error= config.error|| function(e){console.log(e);};
		config.errorConnection= config.errorConnection|| function(e){console.log(e);};
		config.complete= config.complete|| function(e){console.log(e);};
								
		var ajax= document.querySelector( config.selector|| '#update' );
		ajax.action= config.action || ajax.action;
		ajax.model=  config.model  || ajax.model;
        ajax.headers = config.headers ||  '{"Content-Type":"application/x-www-form-urlencoded"}';
				
		ajax.params= JSON.stringify({Data: window.liebre.tools.toFormData(data) } );
								
		ajax.errorConnectionHandler= config.errorConnection;
		ajax.errorHandler= config.error;
		ajax.responseHandler= config.response;
		ajax.completeHandler= config.complete;
		
		ajax.baseUrl= config.baseUrl || ajax.baseUrl || window.liebre.tools.getBaseUrl();	
		ajax.go();
	};
		
	window.liebre.remote.delete=function(data, config){
		var id = (typeof data === 'object')? data.Id: data;
		config = config || {};	
		config.response= config.response|| function(e){console.log(e);};
		config.error= config.error|| function(e){console.log(e);};
		config.errorConnection= config.errorConnection|| function(e){console.log(e);};
		config.complete= config.complete|| function(e){console.log(e);};
								
		var ajax= document.querySelector( config.selector|| '#delete' );
		ajax.action= config.action || ajax.action;
		ajax.model=  config.model  || ajax.model;
        ajax.headers = config.headers ||  '{"Content-Type":"application/x-www-form-urlencoded"}';
				
		ajax.params= JSON.stringify({Id: id } );
								
		ajax.errorConnectionHandler= config.errorConnection;
		ajax.errorHandler= config.error;
		ajax.responseHandler= config.response;
		ajax.completeHandler= config.complete;
		
		ajax.baseUrl= config.baseUrl || ajax.baseUrl || window.liebre.tools.getBaseUrl();	
		ajax.go();
	};
	
	window.liebre.remote.updateCuestionario=function(data, config){
		config = config || {};
		config.model = config.model|| 'cuestionario';
		window.liebre.remote.update(data, config);
	};
	
	// ver 1
	var schema = {
		stores: [{
			name: 'Guia',    // required. object store name or TABLE name
			keyPath: ['Respuesta.IdDiagnostico','Respuesta.IdGuia'],    // tomar de Diagnostico 
			autoIncrement: false, // if true, key will be automatically created
		},{
			name: 'Pregunta',    // required. object store name or TABLE name
			keyPath: ['Respuesta.IdDiagnostico','Respuesta.IdPregunta'],    // tomar de Diagnostico 
			autoIncrement: false, // if true, key will be automatically created
			indexes:[{
				name:'Pregunta.IdCapitulo',
				keyPath:['Respuesta.IdDiagnostico','Pregunta.IdCapitulo']
			}]
		},{
			name: 'Descarga',    // required. object store name or TABLE name // cambiar a 'Cuestionario'
			keyPath: 'Descarga.Token',    // keyPath.
			autoIncrement: false, // if true, key will be automatically created
			indexes: [{
				name: 'Diagnostico.Id', // optional
				keyPath: 'Diagnostico.Id',
				unique: true, // optional, default to false
				multiEntry: false // optional, default to false
			}]
		}]
	};
	
	window.liebre._storage= new window.liebre.IndexStorage('sgsst-test', schema);
	window.liebre._storage.open();
	//
	window.liebre.deleteCuestionario=function(cuestionario, complete){
		complete=complete||function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Cuestionario Eliminado OK',
				data: {
					Respuestas:[],
					RespuestasGuias:[]
				}
			};
			var kv= window.ydn.db.KeyRange.starts([cuestionario.Diagnostico.Id]);
			db.remove('Pregunta', 'Pregunta.IdCapitulo', kv)
			.done(function(){
				db.remove('Guia',  kv)
				.done(function(){
					db.remove('Descarga', cuestionario.Descarga.Token)
					.done(function(){
						__ready=true;
					})
					.fail(function(e){
						doError('Error al eliminar Cuestionario',e);						
					});
				})
				.fail(function(e){
					doError('Error al eliminar Guias',e);
				});
			})
			.fail(function(e){
				doError('Error al eliminar Preguntas',e);
			});
			
			var doError= function(m,e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg=m || 'Error al eliminar Cuestionario!' ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg + e.target.error.name + ' ' +
						e.target.error.message;
                }
				__ready=true;
			};
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
	
	window.liebre.getPreguntasGuias=function(cuestionario, complete){
		complete=complete||function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Preguntas OK',
				data: {
					Respuestas:[],
					RespuestasGuias:[]
				}
			};
			var kv= window.ydn.db.KeyRange.starts([cuestionario.Diagnostico.Id]);
			db.values('Pregunta', 'Pregunta.IdCapitulo', kv)
			.done(function(d){
				if(d[0]){
					for(var i in d){
						response.data.Respuestas.push(d[i].Respuesta);
					}
				}
				db.values('Guia',  kv)
				.done(function(d){
					if(d[0]){
						for(var i in d){
							response.data.RespuestasGuias.push(d[i].Respuesta);
						}
					}
					__ready=true;
				})
				.fail(function(e){
					doError('Error al leer Guias',e);
				});
			})
			.fail(function(e){
				doError('Error al leer Preguntas',e);
			});
			
			var doError= function(m,e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg=m || 'Error al leer Respuestas!' ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg + e.target.error.name + ' ' +
						e.target.error.message;
                }
				__ready=true;
			};
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();		
		});
	};
		
	window.liebre.getPreguntas=function(cuestionario, capitulo, complete){
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Preguntas OK',
				data:[]
			};
			var kv= window.ydn.db.KeyRange.only([cuestionario.Diagnostico.Id, capitulo.Id]);
			db.values('Pregunta', 'Pregunta.IdCapitulo', kv)
			.done(function(aData){
				if(aData[0]){
					response.data= aData;
					__ready=true;
				}
			})
			.fail(function(e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg='Eror al leer Preguntas' ;
				__ready=true;
			});
			
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
	
	window.liebre.getGuias=function(cuestionario, ids, complete){
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Preguntas OK',
				data:[]
			};
			var kv= [];//ydn.db.KeyRange.only([diagnostico.Diagnostico.Id, capitulo.Id]);
			for( var id in ids){
				kv.push([cuestionario.Diagnostico.Id, ids[id]]);
			}
			db.values('Guia',  kv)
			.done(function(aData){
				if(aData[0]){
					response.data= aData;
					__ready=true;
				}
			})
			.fail(function(e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg='Eror al leer Guias' ;
				__ready=true;
			});
			
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
		
	// complete : function({status: 'ok' ||'error',  error: null|| error, msg:'' })
	window.liebre.getCuestionarios=function(complete){
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Cuestionarios OK',
				data:[]
			};
			db.values('Descarga')
			.done(function(aData){
				if(aData[0]){
					response.data= aData;					
				}
				__ready=true;
			})
			.fail(function(e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg='Eror al leer Cuestionarios' ;
				__ready=true;
			});
			
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
	
	window.liebre.putCuestionario=function(cuestionario,complete){
		complete = complete || function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Custionario Actualizado OK',
				data:[]
			};
			db.put('Descarga', cuestionario)
			.done(function(key){
				response.data= key;
				__ready=true;
				
			})
			.fail(function(e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg='Eror al actualizar Cuestionarios' ;
				__ready=true;
			});
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
	
	window.liebre.putRespuesta=function(respuesta,complete){
		complete = complete || function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Respuesta OK',
				data:[]
			};
			db.values('Pregunta',  [[respuesta.IdDiagnostico, respuesta.IdPregunta]])
			.done(function(aData){
				if(aData[0]){
					aData[0].Respuesta= respuesta;
					db.put('Pregunta', aData[0])
					.done(function(){
						response.data= aData;
						__ready=true;
					})
					.fail(function(e){
						doError('',e );
					});
				}
				else{
					doError('Error al leer Respuestas. No existe registro');
				}
			})
			.fail(function(e){
				doError('Error al leer Respuestas',e);
			});
			
			var doError= function(m,e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg=m || 'Actualización de la Respuesta fallida!' ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg + e.target.error.name + ' ' +
						e.target.error.message;
                }
				__ready=true;
			};
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
	
	window.liebre.putRespuestaGuia=function(respuesta,complete){
		complete = complete || function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Respuesta Guia OK',
				data:[]
			};
			db.values('Guia',  [[respuesta.IdDiagnostico, respuesta.IdGuia]])
			.done(function(aData){
				if(aData[0]){
					aData[0].Respuesta= respuesta;
					db.put('Guia', aData[0])
					.done(function(){
						response.data= aData;
						__ready=true;
					})
					.fail(function(e){
						doError('',e );
					});				
				}
				else{
					doError('Error al leer RespuestasGuias. No existe registro');
				}
			})
			.fail(function(e){
				doError('Error al leer RespuestasGuias',e);
			});
			
			var doError= function(m,e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg=m || 'Actualización de la RespuestaGuia fallida!' ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg + e.target.error.name + ' ' + e.target.error.message;
                }
				__ready=true;
			};
			
			(function(){
				var tId = setInterval(function() {
					if ( __ready) {
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
		
	window.liebre.postCuestionario=function(data, complete){
		window.liebre._storage.execute(function(db){
			data.Descarga.Estado='grey';
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Cuestionario Instalado!',
				data:{}
			};
						
			data.Descarga.Fecha=window.liebre.tools.formatDate(data.Descarga.Fecha);
			data.Plantilla.FechaInicial=window.liebre.tools.formatDate(data.Plantilla.FechaInicial);
			data.Plantilla.FechaFinal=window.liebre.tools.formatDate(data.Plantilla.FechaFinal);
			
			for (var cap in data.Capitulos){
				data.Capitulos[cap].Current=0;
			}
				
			var kv= window.ydn.db.KeyRange.only(data.Diagnostico.Id);
			db.values('Descarga','Diagnostico.Id',  kv)
			.done(function(aData){
				var r =aData[0];
				if(!r){
					db.put('Descarga',data)
					.done(function(){
						db.put('Guia', data.Guias)
						.done(function(){
							data.Guias=[];
							db.put('Descarga', data);
							db.put('Pregunta', data.Preguntas)
							.done(function(){
								data.Descarga.Estado='red';
								data.Preguntas=[];
								db.put('Descarga', data)
								.done(function(){response.data=data;  __ready=true;})
								.fail(function(e){ doError('Instalacion fallida! (Descarga Read)',e);});
							})
							.fail(function(e){
								doError('Instalacion fallida! (Preguntas)',e);
							});
						})
						.fail(function(e){
							doError('Instalacion fallida! (Guias)',e);
						});
					})
					.fail(function(e) {
						doError('Instalacion fallida! (Descarga).',e);
					});
				}
				else{
					doError('Instalacion fallida! Borre la Descarga con id:'+ data.Descarga.Id);
				}
			})
			.fail(function(e){
				doError('Instalacion fallida. (Buscar Diagnostico:'+ data.Diagnostico.Id+')',e);
			});
			
			var doError= function(m,e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg=m || 'Instalacion fallida!' ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg + e.target.error.name + ' ' + e.target.error.message;
                }
				__ready=true;
			};
			
			(function(){
				var tId = setInterval(function() {
					if ( __ready){
						onReady();
					}
				}, 11);
				function onReady(){
					clearInterval(tId);
					if(complete){
						complete(response);
					}
				}
			})();
		});
	};
	
	
	document.addEventListener('polymer-ready', function() {
	  // Perform some behaviour
	  console.log('Polymer is ready to rock!');
	  setTimeout(function() { window.scrollTo(0, 1); }, 1);
	  	  
	  var ajax= document.querySelector('#read');
	  ajax.addEventListener('read-response', function(e){
		  if(ajax.responseHandler){
			  ajax.responseHandler(e);
		  }
	  });
	  
	  ajax.addEventListener('read-error-connection', function(e){
		  if(ajax.errorConnectionHandler){
			  ajax.errorConnectionHandler(e);
		  }
	  });
	  
	  ajax.addEventListener('read-error', function(e){
		  if(ajax.errorHandler){
			  ajax.errorHandler(e);
		  }
	  });
	  
	   ajax.addEventListener('read-complete', function(e){
		  if(ajax.completeHandler){
			  ajax.completeHandler(e);
		  }
	  });
	  	  
	  var ajaxCreate= document.querySelector('#create');
	  ajaxCreate.addEventListener('create-response', function(e){
		  if(ajaxCreate.responseHandler){
			  ajaxCreate.responseHandler(e);
		  }
	  });
	  
	  ajaxCreate.addEventListener('create-error-connection', function(e){
		  if(ajaxCreate.errorConnectionHandler){
			  ajaxCreate.errorConnectionHandler(e);
		  }
	  });
	  
	  ajaxCreate.addEventListener('create-error', function(e){
		  if(ajaxCreate.errorHandler){
			  ajaxCreate.errorHandler(e);
		  }
	  });
	  
	   ajaxCreate.addEventListener('create-complete', function(e){
		  if(ajaxCreate.completeHandler){
			  ajaxCreate.completeHandler(e);
		  }
	  });
	  
	  var ajaxupdate= document.querySelector('#update');
	  ajaxupdate.addEventListener('update-response', function(e){
		  if(ajaxupdate.responseHandler){
			  ajaxupdate.responseHandler(e);
		  }
	  });
	  
	  ajaxupdate.addEventListener('update-error-connection', function(e){
		  if(ajaxupdate.errorConnectionHandler){
			  ajaxupdate.errorConnectionHandler(e);
		  }
	  });
	  
	  ajaxupdate.addEventListener('update-error', function(e){
		  if(ajaxupdate.errorHandler){
			  ajaxupdate.errorHandler(e);
		  }
	  });
	  
	   ajaxupdate.addEventListener('update-complete', function(e){
		  if(ajaxupdate.completeHandler){
			  ajaxupdate.completeHandler(e);
		  }
	  });
	  
	  var ajaxdelete= document.querySelector('#delete');
	  ajaxdelete.addEventListener('delete-response', function(e){
		  if(ajaxdelete.responseHandler){
			  ajaxdelete.responseHandler(e);
		  }
	  });
	  
	  ajaxdelete.addEventListener('delete-error-connection', function(e){
		  if(ajaxdelete.errorConnectionHandler){
			  ajaxdelete.errorConnectionHandler(e);
		  }
	  });
	  
	  ajaxdelete.addEventListener('delete-error', function(e){
		  if(ajaxdelete.errorHandler){
			  ajaxdelete.errorHandler(e);
		  }
	  });
		
		ajaxdelete.addEventListener('delete-complete', function(e){
		  if(ajaxdelete.completeHandler){
			  ajaxdelete.completeHandler(e);
		  }
		});
		
	});

// wrap document so it plays nice with other libraries
// http://www.polymer-project.org/platform/shadow-dom.html#wrappers
})(wrap(document));
