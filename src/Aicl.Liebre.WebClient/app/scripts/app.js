(function (document) {
	'use strict';
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
