(function (document) {
	'use strict';
	window.liebre.remote.readCuestionario=function(config){
		config = config || {};
		config.model = config.model|| 'cuestionario';
		window.liebre.remote.read(config);
	};
	window.liebre.remote.updateCuestionario=function(data, config){
		config = config || {};
		config.model = config.model|| 'cuestionario';
		window.liebre.remote.update(data, config);
	};
	
	window.liebre.remote.readCIIU= function(config){
		config = config || {};
		config.model = config.model|| 'ciiu';
		window.liebre.remote.read(config);
	};
	
	window.liebre.remote.readCiudad= function(config){
		config = config || {};
		config.model = config.model|| 'ciudad';
		window.liebre.remote.read(config);
	};
	
	window.liebre.remote.readDepartamento= function(config){
		config = config || {};
		config.model = config.model|| 'departamento';
		window.liebre.remote.read(config);
	};
	window.liebre.remote.readRiesgo= function(config){
		config = config || {};
		config.model = config.model|| 'riesgo';
		window.liebre.remote.read(config);
	};
	window.liebre.remote.readActividadAltoRiesgo= function(config){
		config = config || {};
		config.model = config.model|| 'actividadaltoriesgo';
		window.liebre.remote.read(config);
	};
	window.liebre.remote.readRango= function(config){
		config = config || {};
		config.model = config.model|| 'rango';
		window.liebre.remote.read(config);
	};
	window.liebre.remote.readExterno= function(config){
		config = config || {};
		config.model = config.model|| 'externo';
		window.liebre.remote.read(config);
	};
		
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
					db.remove('Cuestionario', cuestionario.Descarga.Token)
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
			db.values('Pregunta', 'Pregunta.IdCapitulo', kv,2000)
			.done(function(d){
				if(d[0]){
					for(var i in d){
						response.data.Respuestas.push(d[i].Respuesta);
					}
				}
				db.values('Guia',  kv,2000)
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
				}
				__ready=true;
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
				}
				__ready=true;
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
	
	window.liebre.getCIIUs=function(config){
		window.liebre.getData('CIIU', config);
	};
	
	window.liebre.getCiudades=function(config){
		window.liebre.getData('Ciudad', config);
	};
	
	window.liebre.getDepartamentos=function(config){
		window.liebre.getData('Departamento', config);
	};
	
	window.liebre.getRiesgos=function(config){
		window.liebre.getData('Riesgo', config);
	};
	
	window.liebre.getActividadesAltoRiesgo=function(config){
		window.liebre.getData('ActividadAltoRiesgo', config);
	};
	
	window.liebre.getRangos=function(config){
		window.liebre.getData('Rango', config);
	};
	
	window.liebre.getExternos=function(config){
		window.liebre.getData('Externo', config);
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
			db.values('Cuestionario')
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
		cuestionario.Descarga.Fecha= window.liebre.tools.formatDate(cuestionario.Descarga.Fecha);
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Custionario Actualizado OK',
				data:[]
			};
			db.put('Cuestionario', cuestionario)
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
					aData[0].Respuesta=respuesta;
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
			
			for (var g in data.Guias){
				if(data.Guias[g].Respuesta.Valor){
					data.Guias[g].Respuesta.Valor= window.liebre.tools.parseQ(data.Guias[g].Respuesta.Valor);
				}
			}
			db.put('Cuestionario',data)
			.done(function(){
				db.put('Guia', data.Guias)
				.done(function(){
					data.Guias=[];
					db.put('Cuestionario', data);
					db.put('Pregunta', data.Preguntas)
					.done(function(){
						data.Descarga.Estado='red';
						data.Preguntas=[];
						db.put('Cuestionario', data)
						.done(function(){response.data=data;  __ready=true;})
						.fail(function(e){ doError('Instalacion fallida! (Cuestionario put)',e);});
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
				doError('Instalacion fallida! (Cuestionario).',e);
			});			
			var doError= function(m,e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg=m || 'Instalacion fallida!' ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg + ' '+ e.target.error.name + ' ' + e.target.error.message;
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
			
	window.liebre.putCIIU=function(data, complete){
		window.liebre.putData('CIIU', data, complete);
	};
	
	window.liebre.putCiudad=function(data, complete){
		window.liebre.putData('Ciudad', data, complete);
	};
	
	window.liebre.putDepartamento=function(data, complete){
		window.liebre.putData('Departamento', data, complete);
	};
	
	window.liebre.putRiesgo=function(data, complete){
		window.liebre.putData('Riesgo', data, complete);
	};
	
	window.liebre.putActividaAltoRiesgo=function(data, complete){
		window.liebre.putData('ActividaAltoRiesgo', data, complete);
	};
	
	window.liebre.putRango=function(data, complete){
		window.liebre.putData('Rango', data, complete);
	};
	window.liebre.putExterno=function(data, complete){
		window.liebre.putData('Externo', data, complete);
	};
	
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
			name: 'Cuestionario',    // required. object store name or TABLE name // cambiar a 'Cuestionario'
			keyPath: 'Descarga.Token',    // keyPath.
			autoIncrement: false, // if true, key will be automatically created
			indexes: [{
				name: 'Diagnostico.Id', // optional
				keyPath: 'Diagnostico.Id',
				unique: true, // optional, default to false
				multiEntry: false // optional, default to false
			}]
		},{
			name:'CIIU',
			keyPath:'Codigo',
			autoIncrement:false,
			indexes:[{
				name:'Descripcion',
				keyPath:'Descripcion'
			},{
				name:'Seccion',
				keyPath:'Seccion'
			}]
		},{
			name:'Ciudad',
			keyPath:'Codigo',
			autoIncrement:false,
			indexes:[{
				name:'Nombre',
				keyPath:'Nombre'
			},{
				name:'Departamento.Codigo',
				keyPath:'Departamento.Codigo'
			}]
		},{
			name:'Departamento',
			keyPath:'Codigo',
			autoIncrement:false
		},{
			name:'ActividadAltoRiesgo',
			keyPath:'Codigo',
			autoIncrement:false
		},{
			name:'Riesgo',
			keyPath:'Codigo',
			autoIncrement:false
		},{
			name:'Rango',
			keyPath:'Codigo',
			autoIncrement:false
		},{
			name:'Externo',
			keyPath:'Codigo',
			autoIncrement:false
		},{
			name:'tabla',
			keyPath:'id',
			autoIncrement:true,
			indexes:[{
				name:'tipo',
				keyPath:'tipo'
			}]
		},{
			name:'data',
			keyPath:'id',
			autoIncrement:true,
			indexes:[{
				name:'tipo',
				keyPath:'tipo'
			}]
		}]
	};
		
	window.liebre.openStorage('sgsst-q', schema);
		
	document.addEventListener('polymer-ready', function() {
		// Perform some behaviour
		console.log('Polymer is ready to rock: go app!');
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
		var app = document.querySelector('x-app');
		app.go();
	});
	// wrap document so it plays nice with other libraries
	// http://www.polymer-project.org/platform/shadow-dom.html#wrappers
})(wrap(document));
