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
		String.prototype.startsWith = function (str, nocase){
			return (!nocase? this.slice(0, str.length) === str:
					this.slice(0, str.length).toLowerCase() === str.toLowerCase());
		};
	}
	if (typeof String.prototype.endsWith !== 'function') {
		String.prototype.endsWith = function (str, nocase){
			return (!nocase? this.slice(-str.length) === str:
					this.slice(-str.length).toLowerCase()=== str.toLowerCase() );
		};
	}
	String.prototype.contains = function(text, nocase) {
		return ( !nocase? this.indexOf(text) !== -1 : this.toLowerCase().indexOf(text.toLowerCase()) !== -1 );
	};
		
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
					
	window.liebre.tools.encodeQ=function(value){
		return value;
		/*if(typeof value === 'number') {
			return value;
		}
		return encodeURI(JSON.stringify(value));
		*/
	};
	
	window.liebre.tools.getBaseUrl=function(){
		return localStorage.getItem('liebre.baseUrl') ||
			location.protocol.startsWith('http')?
			((location.protocol + '//' + location.host+ '/lbr-api').replace('9000','8080'))
		:'http://liebredemo.apphb.com/lbr-api';
	};
	
	window.liebre.tools.setBaseUrl=function(url){
		localStorage.setItem('liebre.baseUrl', url ||
							 location.protocol.startsWith('http')?
							 ((location.protocol + '//' + location.host+ '/lbr-api').replace('9000','8080'))
							 :'http://liebredemo.apphb.com/lbr-api');
	};
		
	window.liebre.tools.toFormData=function(obj){
		if (typeof obj==='undefined' || obj === null ){
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
	
	window.liebre.tools.isBsonNumberLong=function(v){
		return /^NumberLong\("([^)]+)"\)/.test(v);
	};
	
	window.liebre.tools.parseBsonNumberLong=function(v){
		return JSON.parse(/^NumberLong\("([^)]+)"\)/.exec(v)[1]);
	};
	
	window.liebre.tools.parseQ=function(v){
		var _v = decodeURI(v);
		var fn = JSON.parse;
		if(window.liebre.tools.isBsonNumberLong(_v)){
			fn= window.liebre.tools.parseBsonNumberLong;
		}
		return fn(_v);
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
	
	window.liebre.getData=function(storeName, config){
		config=config||{};
		config.complete=config.complete||function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: storeName+':OK',
				data:[]
			};
			db.values(storeName, null, 10000)
			.done(function(aData){
				response.data= config.filterFn?
					window._.filter(aData[0]?aData:[],config.filterFn):aData[0]?aData:[];
				__ready=true;
			})
			.fail(function(e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg='Eror al leer:'+ storeName ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg+' ' + e.target.error.name + ' ' + e.target.error.message;
                }
				else if(e){
					response.msg= response.msg+'. ' + e.name + ' ' + e.message;
				}
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
					if(config.complete){
						config.complete(response);
					}
				}
			})();
		});
	};
	
	window.liebre.putData=function(storeName,value,complete){
		complete = complete || function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				error:null,
				msg: 'Datos actualizaods OK:'+storeName+'.',
				data:[]
			};
			db.put(storeName, value)
			.done(function(key){
				response.data= key;
				__ready=true;
				
			})
			.fail(function(e){
				console.log('error',e);
				response.status='error';
				response.error=e;
				response.msg='Eror al actualizar datos:' +storeName+'.' ;
				if (e && e.target && e.target.error) {
					response.msg= response.msg+' ' + e.target.error.name + ' ' + e.target.error.message;
                }
				else if(e){
					response.msg= response.msg+' ' + e.name + ' ' + e.message;
				}
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
	
	window.liebre.countData=function(storeName, complete){
		complete = complete || function(){};
		window.liebre._storage.execute(function(db){
			var __ready=false;
			var response= {
				status:'ok',
				number: null
			};
			try{
				db.count(storeName)
				.done(function(number){
					response.number= number;
					__ready=true;
				});
			}
			catch(e){
				response.status='error';
				response.error=e;
				response.msg='Error countData.';
				if (e && e.target && e.target.error) {
					response.msg= response.msg+' ' + e.target.error.name + ' ' + e.target.error.message;
                }
				else if(e){
					response.msg= response.msg+' ' + e.name + '. ' + e.message;
				}
				__ready=true;
			}
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
	
	
	window.liebre.IndexStorage = (function () {
		function IndexStorage(name, schema, options) {
			this.__name=name||'default-storage';
			this.__schema=schema||{};
			this.__options= options||{mechanisms: ['websql', 'indexeddb']};
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
				var times=1000000;
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

	window.liebre.openStorage=function(name, schema ){
		window.liebre._storage= new window.liebre.IndexStorage(name, schema);
		window.liebre._storage.open();
	};

	
// wrap document so it plays nice with other libraries
// http://www.polymer-project.org/platform/shadow-dom.html#wrappers
})(wrap(document));
