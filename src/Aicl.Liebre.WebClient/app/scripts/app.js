(function (document) {
	'use strict';
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
		
	if (!navigator.getUserMedia) {
		navigator.getUserMedia = navigator.webkitGetUserMedia || navigator.mozGetUserMedia || 
			navigator.msGetUserMedia;
	}
	
	if (!window.URL) {
		window.URL =  window.webkitURL || window.msURL || window.oURL;
	}
		
	window.liebre={};
	window.liebre.tools={};
	window.liebre.remote={};
	
	window.liebre.tools.getBaseUrl=function(){
		return localStorage.getItem('liebre.baseUrl') ||
			( (location.protocol + '//' + location.host+ '/lbr-api').replace('9000','8080'));
	};
	
	window.liebre.tools.setBaseUrl=function(url){
		localStorage.setItem('liebre.baseUrl', url ||
							 ( (location.protocol + '//' + location.host+ '/lbr-api').replace('9000','8080')));
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
