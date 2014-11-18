!function(a){"use strict";navigator.getUserMedia||(navigator.getUserMedia=navigator.webkitGetUserMedia||navigator.mozGetUserMedia||navigator.msGetUserMedia),window.URL||(window.URL=window.webkitURL||window.msURL||window.oURL),Date.prototype.toDateInputValue=function(){var a=new Date(this);return a.setMinutes(this.getMinutes()-this.getTimezoneOffset()),a.toJSON().slice(0,10)},Date.prototype.toTimeInputValue=function(){var a=new Date(this);return a.toTimeString().slice(0,8)},Date.prototype.toMsFormat=function(){var a=new Date(this);return"/Date("+a.getTime()+")/"},Date.prototype.toFilename=function(){return this.toDateInputValue().replace(/-/g,"")+"_"+this.toTimeInputValue().replace(/:/g,"")},"function"!=typeof String.prototype.startsWith&&(String.prototype.startsWith=function(a,b){return b?this.slice(0,a.length).toLowerCase()===a.toLowerCase():this.slice(0,a.length)===a}),"function"!=typeof String.prototype.endsWith&&(String.prototype.endsWith=function(a,b){return b?this.slice(-a.length).toLowerCase()===a.toLowerCase():this.slice(-a.length)===a}),String.prototype.contains=function(a,b){return b?-1!==this.toLowerCase().indexOf(a.toLowerCase()):-1!==this.indexOf(a)},Object.clone=function(a){var b=a instanceof Array?[]:{};if(null===a||a instanceof Date||!(a instanceof Array||"object"==typeof a))return a;for(var c in a)b[c]=a[c]&&"object"==typeof a[c]?Object.clone(a[c]):a[c];return b},Object.compare=function(a,b){if(!a)return!b;if(null===a)return null===b?!0:!1;if(a instanceof Date)return b instanceof Date?a.toString()===b.toString():!1;if(!(a instanceof Array||"object"==typeof a))return a instanceof Array||"object"==typeof a?!1:a===b;if(Object.getOwnPropertyNames(a).length!==Object.getOwnPropertyNames(b||{}).length)return!1;for(var c in a)if("object"==typeof a[c]){if(!Object.compare(a[c],b[c]))return!1}else if(a[c]!==b[c])return!1;return!0},window.liebre={},window.liebre.tools={},window.liebre.remote={},window.liebre.tools.encodeQ=function(a){return a},window.liebre.tools.getBaseUrl=function(){return localStorage.getItem("liebre.baseUrl")||location.protocol.startsWith("http")?(location.protocol+"//"+location.host+"/lbr-api").replace("9000","8080"):"http://liebredemo.apphb.com/lbr-api"},window.liebre.tools.setBaseUrl=function(a){localStorage.setItem("liebre.baseUrl",a||location.protocol.startsWith("http")?(location.protocol+"//"+location.host+"/lbr-api").replace("9000","8080"):"http://liebredemo.apphb.com/lbr-api")},window.liebre.tools.toFormData=function(a){if("undefined"==typeof a||null===a)return null;if(a instanceof Date)return'"'+a.toISOString()+'"';if(!(a instanceof Array||"object"==typeof a))return"string"==typeof a?'"'+a+'"':a;if(0===Object.getOwnPropertyNames(a).length)return"{}";var b=a instanceof Array?"[":"{";for(var c in a)if(a[c]instanceof Date)b=b+(a instanceof Array?'"':c+':"')+a[c].toISOString()+'",';else if(a[c]instanceof Array){var d="[";for(var e in a[c])d=d+window.liebre.tools.toFormData(a[c][e])+",";b=b+(a instanceof Array?"":c+":")+(a[c].length>0?d.replace(/,([^,]*)$/,"]$1")+",":"[],")}else if("object"==typeof a[c])b=b+(a instanceof Array?"":c+":")+window.liebre.tools.toFormData(a[c])+",";else{var f="string"==typeof a[c]?'"':"";b=b+(a instanceof Array?f:c+":"+f)+a[c]+f+","}return b.replace(/,([^,]*)$/,(a instanceof Array?"]":"}")+"$1")},window.liebre.tools.convertToJsDate=function(a){return a?"string"!=typeof a?a:new Date(parseFloat(/Date\(([^)]+)\)/.exec(a)[1])):null},window.liebre.tools.isBsonNumberLong=function(a){return/^NumberLong\("([^)]+)"\)/.test(a)},window.liebre.tools.parseBsonNumberLong=function(a){return JSON.parse(/^NumberLong\("([^)]+)"\)/.exec(a)[1])},window.liebre.tools.parseQ=function(a){var b=decodeURI(a),c=JSON.parse;return window.liebre.tools.isBsonNumberLong(b)&&(c=window.liebre.tools.parseBsonNumberLong),c(b)},window.liebre.tools.formatDate=function(a){if(!a)return"";var b=window.liebre.tools.convertToJsDate(a);return b.toDateInputValue()},window.liebre.tools.blink=function(a,b){b=b||{},a.style.backgroundColor=b.backgroudColor||"white",a.style.color=b.backgroudColor||"rgb(8, 13, 49)",setTimeout(function(){a.style.backgroundColor="",a.style.color=""},b.wait||100)},window.liebre.remote.save=function(a,b){a.Id?window.liebre.remote.update(a,b):window.liebre.remote.create(a,b)},window.liebre.remote.read=function(b){b=b||{},b.response=b.response||function(a){console.log(a)},b.error=b.error||function(a){console.log(a)},b.errorConnection=b.errorConnection||function(a){console.log(a)},b.complete=b.complete||function(a){console.log(a)};var c=a.querySelector(b.selector||"#read");c.action=b.action||c.action,c.model=b.model||c.model,c.urlparams=b.urlparams||c.urlparams||{},c.urlparams.format=c.urlparams.format||"json",c.errorConnectionHandler=b.errorConnection,c.errorHandler=b.error,c.responseHandler=b.response,c.completeHandler=b.complete,c.baseUrl=b.baseUrl||c.baseUrl||window.liebre.tools.getBaseUrl(),c.go()},window.liebre.remote.create=function(b,c){c=c||{},c.response=c.response||function(a){console.log(a)},c.error=c.error||function(a){console.log(a)},c.errorConnection=c.errorConnection||function(a){console.log(a)},c.complete=c.complete||function(a){console.log(a)};var d=a.querySelector(c.selector||"#create");d.action=c.action||d.action,d.model=c.model||d.model,d.headers=c.headers||'{"Content-Type":"application/x-www-form-urlencoded"}',d.params=JSON.stringify({Data:window.liebre.tools.toFormData(b)}),d.errorConnectionHandler=c.errorConnection,d.errorHandler=c.error,d.responseHandler=c.response,d.completeHandler=c.complete,d.baseUrl=c.baseUrl||d.baseUrl||window.liebre.tools.getBaseUrl(),d.go()},window.liebre.remote.update=function(b,c){c=c||{},c.response=c.response||function(a){console.log(a)},c.error=c.error||function(a){console.log(a)},c.errorConnection=c.errorConnection||function(a){console.log(a)},c.complete=c.complete||function(a){console.log(a)};var d=a.querySelector(c.selector||"#update");d.action=c.action||d.action,d.model=c.model||d.model,d.headers=c.headers||'{"Content-Type":"application/x-www-form-urlencoded"}',d.params=JSON.stringify({Data:window.liebre.tools.toFormData(b)}),d.errorConnectionHandler=c.errorConnection,d.errorHandler=c.error,d.responseHandler=c.response,d.completeHandler=c.complete,d.baseUrl=c.baseUrl||d.baseUrl||window.liebre.tools.getBaseUrl(),d.go()},window.liebre.remote.delete=function(b,c){var d="object"==typeof b?b.Id:b;c=c||{},c.response=c.response||function(a){console.log(a)},c.error=c.error||function(a){console.log(a)},c.errorConnection=c.errorConnection||function(a){console.log(a)},c.complete=c.complete||function(a){console.log(a)};var e=a.querySelector(c.selector||"#delete");e.action=c.action||e.action,e.model=c.model||e.model,e.headers=c.headers||'{"Content-Type":"application/x-www-form-urlencoded"}',e.params=JSON.stringify({Id:d}),e.errorConnectionHandler=c.errorConnection,e.errorHandler=c.error,e.responseHandler=c.response,e.completeHandler=c.complete,e.baseUrl=c.baseUrl||e.baseUrl||window.liebre.tools.getBaseUrl(),e.go()},window.liebre.getData=function(a,b){b=b||{},b.complete=b.complete||function(){},window.liebre._storage.execute(function(c){var d=!1,e={status:"ok",error:null,msg:a+":OK",data:[]};c.values(a,null,1e4).done(function(a){e.data=b.filterFn?window._.filter(a[0]?a:[],b.filterFn):a[0]?a:[],d=!0}).fail(function(b){console.log("error",b),e.status="error",e.error=b,e.msg="Eror al leer:"+a,b&&b.target&&b.target.error?e.msg=e.msg+" "+b.target.error.name+" "+b.target.error.message:b&&(e.msg=e.msg+". "+b.name+" "+b.message),d=!0}),function(){function a(){clearInterval(c),b.complete&&b.complete(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.putData=function(a,b,c){c=c||function(){},window.liebre._storage.execute(function(d){var e=!1,f={status:"ok",error:null,msg:"Datos actualizaods OK:"+a+".",data:[]};d.put(a,b).done(function(a){f.data=a,e=!0}).fail(function(b){console.log("error",b),f.status="error",f.error=b,f.msg="Eror al actualizar datos:"+a+".",b&&b.target&&b.target.error?f.msg=f.msg+" "+b.target.error.name+" "+b.target.error.message:b&&(f.msg=f.msg+" "+b.name+" "+b.message),e=!0}),function(){function a(){clearInterval(b),c&&c(f)}var b=setInterval(function(){e&&a()},11)}()})},window.liebre.countData=function(a,b){b=b||function(){},window.liebre._storage.execute(function(c){var d=!1,e={status:"ok",number:null};try{c.count(a).done(function(a){e.number=a,d=!0})}catch(f){e.status="error",e.error=f,e.msg="Error countData.",f&&f.target&&f.target.error?e.msg=e.msg+" "+f.target.error.name+" "+f.target.error.message:f&&(e.msg=e.msg+" "+f.name+". "+f.message),d=!0}!function(){function a(){clearInterval(c),b&&b(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.IndexStorage=function(){function a(a,b,c){this.__name=a||"default-storage",this.__schema=b||{},this.__options=c||{mechanisms:["websql","indexeddb"]},this.__db=null,this.__ready=!1}return Object.defineProperty(a.prototype,"name",{get:function(){return this.__name},enumerable:!0,configurable:!0}),Object.defineProperty(a.prototype,"opened",{get:function(){return null!==this.__db},enumerable:!0,configurable:!0}),a.prototype.open=function(){var a=this;null===a.__db&&(a.__db=new window.ydn.db.Storage(a.__name,a.__schema,a.__options),a.__db.onReady(function(b){if(b)throw a.__db=null,a.__ready=!1,b.target.error&&console.log("Error due to: "+b.target.error.name+" "+b.target.error.message),b;a.__ready=!0}))},a.prototype.execute=function(a){var b=this;return b.__ready?void a(b.__db):void function(){function c(){clearInterval(e),a(b.__db)}var d=1e6,e=setInterval(function(){b.__ready?c():(d--,0>=d&&clearInterval(e))},11)}()},a.prototype.close=function(){null!==this.__db&&this.__db.close()},a}(),window.liebre.openStorage=function(a,b){window.liebre._storage=new window.liebre.IndexStorage(a,b),window.liebre._storage.open()}}(wrap(document)),function(a){"use strict";window.liebre.remote.readCuestionario=function(a){a=a||{},a.model=a.model||"cuestionario",window.liebre.remote.read(a)},window.liebre.remote.updateCuestionario=function(a,b){b=b||{},b.model=b.model||"cuestionario",window.liebre.remote.update(a,b)},window.liebre.remote.readCIIU=function(a){a=a||{},a.model=a.model||"ciiu",window.liebre.remote.read(a)},window.liebre.remote.readCiudad=function(a){a=a||{},a.model=a.model||"ciudad",window.liebre.remote.read(a)},window.liebre.remote.readDepartamento=function(a){a=a||{},a.model=a.model||"departamento",window.liebre.remote.read(a)},window.liebre.remote.readRiesgo=function(a){a=a||{},a.model=a.model||"riesgo",window.liebre.remote.read(a)},window.liebre.remote.readActividadAltoRiesgo=function(a){a=a||{},a.model=a.model||"actividadaltoriesgo",window.liebre.remote.read(a)},window.liebre.remote.readRango=function(a){a=a||{},a.model=a.model||"rango",window.liebre.remote.read(a)},window.liebre.remote.readExterno=function(a){a=a||{},a.model=a.model||"externo",window.liebre.remote.read(a)},window.liebre.deleteCuestionario=function(a,b){b=b||function(){},window.liebre._storage.execute(function(c){var d=!1,e={status:"ok",error:null,msg:"Cuestionario Eliminado OK",data:{Respuestas:[],RespuestasGuias:[]}},f=window.ydn.db.KeyRange.starts([a.Diagnostico.Id]);c.remove("Pregunta","Pregunta.IdCapitulo",f).done(function(){c.remove("Guia",f).done(function(){c.remove("Cuestionario",a.Descarga.Token).done(function(){d=!0}).fail(function(a){g("Error al eliminar Cuestionario",a)})}).fail(function(a){g("Error al eliminar Guias",a)})}).fail(function(a){g("Error al eliminar Preguntas",a)});var g=function(a,b){console.log("error",b),e.status="error",e.error=b,e.msg=a||"Error al eliminar Cuestionario!",b&&b.target&&b.target.error&&(e.msg=e.msg+b.target.error.name+" "+b.target.error.message),d=!0};!function(){function a(){clearInterval(c),b&&b(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.getPreguntasGuias=function(a,b){b=b||function(){},window.liebre._storage.execute(function(c){var d=!1,e={status:"ok",error:null,msg:"Preguntas OK",data:{Respuestas:[],RespuestasGuias:[]}},f=window.ydn.db.KeyRange.starts([a.Diagnostico.Id]);c.values("Pregunta","Pregunta.IdCapitulo",f).done(function(a){if(a[0])for(var b in a)e.data.Respuestas.push(a[b].Respuesta);c.values("Guia",f).done(function(a){if(a[0])for(var b in a)e.data.RespuestasGuias.push(a[b].Respuesta);d=!0}).fail(function(a){g("Error al leer Guias",a)})}).fail(function(a){g("Error al leer Preguntas",a)});var g=function(a,b){console.log("error",b),e.status="error",e.error=b,e.msg=a||"Error al leer Respuestas!",b&&b.target&&b.target.error&&(e.msg=e.msg+b.target.error.name+" "+b.target.error.message),d=!0};!function(){function a(){clearInterval(c),b&&b(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.getPreguntas=function(a,b,c){window.liebre._storage.execute(function(d){var e=!1,f={status:"ok",error:null,msg:"Preguntas OK",data:[]},g=window.ydn.db.KeyRange.only([a.Diagnostico.Id,b.Id]);d.values("Pregunta","Pregunta.IdCapitulo",g).done(function(a){a[0]&&(f.data=a),e=!0}).fail(function(a){console.log("error",a),f.status="error",f.error=a,f.msg="Eror al leer Preguntas",e=!0}),function(){function a(){clearInterval(b),c&&c(f)}var b=setInterval(function(){e&&a()},11)}()})},window.liebre.getGuias=function(a,b,c){window.liebre._storage.execute(function(d){var e=!1,f={status:"ok",error:null,msg:"Preguntas OK",data:[]},g=[];for(var h in b)g.push([a.Diagnostico.Id,b[h]]);d.values("Guia",g).done(function(a){a[0]&&(f.data=a),e=!0}).fail(function(a){console.log("error",a),f.status="error",f.error=a,f.msg="Eror al leer Guias",e=!0}),function(){function a(){clearInterval(b),c&&c(f)}var b=setInterval(function(){e&&a()},11)}()})},window.liebre.getCIIUs=function(a){window.liebre.getData("CIIU",a)},window.liebre.getCiudades=function(a){window.liebre.getData("Ciudad",a)},window.liebre.getDepartamentos=function(a){window.liebre.getData("Departamento",a)},window.liebre.getRiesgos=function(a){window.liebre.getData("Riesgo",a)},window.liebre.getActividadesAltoRiesgo=function(a){window.liebre.getData("ActividadAltoRiesgo",a)},window.liebre.getRangos=function(a){window.liebre.getData("Rango",a)},window.liebre.getExternos=function(a){window.liebre.getData("Externo",a)},window.liebre.getCuestionarios=function(a){window.liebre._storage.execute(function(b){var c=!1,d={status:"ok",error:null,msg:"Cuestionarios OK",data:[]};b.values("Cuestionario").done(function(a){a[0]&&(d.data=a),c=!0}).fail(function(a){console.log("error",a),d.status="error",d.error=a,d.msg="Eror al leer Cuestionarios",c=!0}),function(){function b(){clearInterval(e),a&&a(d)}var e=setInterval(function(){c&&b()},11)}()})},window.liebre.putCuestionario=function(a,b){b=b||function(){},a.Descarga.Fecha=window.liebre.tools.formatDate(a.Descarga.Fecha),window.liebre._storage.execute(function(c){var d=!1,e={status:"ok",error:null,msg:"Custionario Actualizado OK",data:[]};c.put("Cuestionario",a).done(function(a){e.data=a,d=!0}).fail(function(a){console.log("error",a),e.status="error",e.error=a,e.msg="Eror al actualizar Cuestionarios",d=!0}),function(){function a(){clearInterval(c),b&&b(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.putRespuesta=function(a,b){b=b||function(){},window.liebre._storage.execute(function(c){var d=!1,e={status:"ok",error:null,msg:"Respuesta OK",data:[]};c.values("Pregunta",[[a.IdDiagnostico,a.IdPregunta]]).done(function(b){b[0]?(b[0].Respuesta=a,c.put("Pregunta",b[0]).done(function(){e.data=b,d=!0}).fail(function(a){f("",a)})):f("Error al leer Respuestas. No existe registro")}).fail(function(a){f("Error al leer Respuestas",a)});var f=function(a,b){console.log("error",b),e.status="error",e.error=b,e.msg=a||"Actualización de la Respuesta fallida!",b&&b.target&&b.target.error&&(e.msg=e.msg+b.target.error.name+" "+b.target.error.message),d=!0};!function(){function a(){clearInterval(c),b&&b(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.putRespuestaGuia=function(a,b){b=b||function(){},window.liebre._storage.execute(function(c){var d=!1,e={status:"ok",error:null,msg:"Respuesta Guia OK",data:[]};c.values("Guia",[[a.IdDiagnostico,a.IdGuia]]).done(function(b){b[0]?(b[0].Respuesta=a,c.put("Guia",b[0]).done(function(){e.data=b,d=!0}).fail(function(a){f("",a)})):f("Error al leer RespuestasGuias. No existe registro")}).fail(function(a){f("Error al leer RespuestasGuias",a)});var f=function(a,b){console.log("error",b),e.status="error",e.error=b,e.msg=a||"Actualización de la RespuestaGuia fallida!",b&&b.target&&b.target.error&&(e.msg=e.msg+b.target.error.name+" "+b.target.error.message),d=!0};!function(){function a(){clearInterval(c),b&&b(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.postCuestionario=function(a,b){window.liebre._storage.execute(function(c){a.Descarga.Estado="grey";var d=!1,e={status:"ok",error:null,msg:"Cuestionario Instalado!",data:{}};a.Descarga.Fecha=window.liebre.tools.formatDate(a.Descarga.Fecha),a.Plantilla.FechaInicial=window.liebre.tools.formatDate(a.Plantilla.FechaInicial),a.Plantilla.FechaFinal=window.liebre.tools.formatDate(a.Plantilla.FechaFinal);for(var f in a.Capitulos)a.Capitulos[f].Current=0;for(var g in a.Guias)a.Guias[g].Respuesta.Valor&&(a.Guias[g].Respuesta.Valor=window.liebre.tools.parseQ(a.Guias[g].Respuesta.Valor));c.put("Cuestionario",a).done(function(){c.put("Guia",a.Guias).done(function(){a.Guias=[],c.put("Cuestionario",a),c.put("Pregunta",a.Preguntas).done(function(){a.Descarga.Estado="red",a.Preguntas=[],c.put("Cuestionario",a).done(function(){e.data=a,d=!0}).fail(function(a){h("Instalacion fallida! (Cuestionario put)",a)})}).fail(function(a){h("Instalacion fallida! (Preguntas)",a)})}).fail(function(a){h("Instalacion fallida! (Guias)",a)})}).fail(function(a){h("Instalacion fallida! (Cuestionario).",a)});var h=function(a,b){console.log("error",b),e.status="error",e.error=b,e.msg=a||"Instalacion fallida!",b&&b.target&&b.target.error&&(e.msg=e.msg+" "+b.target.error.name+" "+b.target.error.message),d=!0};!function(){function a(){clearInterval(c),b&&b(e)}var c=setInterval(function(){d&&a()},11)}()})},window.liebre.putCIIU=function(a,b){window.liebre.putData("CIIU",a,b)},window.liebre.putCiudad=function(a,b){window.liebre.putData("Ciudad",a,b)},window.liebre.putDepartamento=function(a,b){window.liebre.putData("Departamento",a,b)},window.liebre.putRiesgo=function(a,b){window.liebre.putData("Riesgo",a,b)},window.liebre.putActividaAltoRiesgo=function(a,b){window.liebre.putData("ActividaAltoRiesgo",a,b)},window.liebre.putRango=function(a,b){window.liebre.putData("Rango",a,b)},window.liebre.putExterno=function(a,b){window.liebre.putData("Externo",a,b)};var b={stores:[{name:"Guia",keyPath:["Respuesta.IdDiagnostico","Respuesta.IdGuia"],autoIncrement:!1},{name:"Pregunta",keyPath:["Respuesta.IdDiagnostico","Respuesta.IdPregunta"],autoIncrement:!1,indexes:[{name:"Pregunta.IdCapitulo",keyPath:["Respuesta.IdDiagnostico","Pregunta.IdCapitulo"]}]},{name:"Cuestionario",keyPath:"Descarga.Token",autoIncrement:!1,indexes:[{name:"Diagnostico.Id",keyPath:"Diagnostico.Id",unique:!0,multiEntry:!1}]},{name:"CIIU",keyPath:"Codigo",autoIncrement:!1,indexes:[{name:"Descripcion",keyPath:"Descripcion"},{name:"Seccion",keyPath:"Seccion"}]},{name:"Ciudad",keyPath:"Codigo",autoIncrement:!1,indexes:[{name:"Nombre",keyPath:"Nombre"},{name:"Departamento.Codigo",keyPath:"Departamento.Codigo"}]},{name:"Departamento",keyPath:"Codigo",autoIncrement:!1},{name:"ActividadAltoRiesgo",keyPath:"Codigo",autoIncrement:!1},{name:"Riesgo",keyPath:"Codigo",autoIncrement:!1},{name:"Rango",keyPath:"Codigo",autoIncrement:!1},{name:"Externo",keyPath:"Codigo",autoIncrement:!1},{name:"tabla",keyPath:"id",autoIncrement:!0,indexes:[{name:"tipo",keyPath:"tipo"}]},{name:"data",keyPath:"id",autoIncrement:!0,indexes:[{name:"tipo",keyPath:"tipo"}]}]};window.liebre.openStorage("sgsst-q",b),a.addEventListener("polymer-ready",function(){console.log("Polymer is ready to rock: go app!");var b=a.querySelector("#read");b.addEventListener("read-response",function(a){b.responseHandler&&b.responseHandler(a)}),b.addEventListener("read-error-connection",function(a){b.errorConnectionHandler&&b.errorConnectionHandler(a)}),b.addEventListener("read-error",function(a){b.errorHandler&&b.errorHandler(a)}),b.addEventListener("read-complete",function(a){b.completeHandler&&b.completeHandler(a)});var c=a.querySelector("#create");c.addEventListener("create-response",function(a){c.responseHandler&&c.responseHandler(a)}),c.addEventListener("create-error-connection",function(a){c.errorConnectionHandler&&c.errorConnectionHandler(a)}),c.addEventListener("create-error",function(a){c.errorHandler&&c.errorHandler(a)}),c.addEventListener("create-complete",function(a){c.completeHandler&&c.completeHandler(a)});var d=a.querySelector("#update");d.addEventListener("update-response",function(a){d.responseHandler&&d.responseHandler(a)}),d.addEventListener("update-error-connection",function(a){d.errorConnectionHandler&&d.errorConnectionHandler(a)}),d.addEventListener("update-error",function(a){d.errorHandler&&d.errorHandler(a)}),d.addEventListener("update-complete",function(a){d.completeHandler&&d.completeHandler(a)});var e=a.querySelector("#delete");e.addEventListener("delete-response",function(a){e.responseHandler&&e.responseHandler(a)}),e.addEventListener("delete-error-connection",function(a){e.errorConnectionHandler&&e.errorConnectionHandler(a)}),e.addEventListener("delete-error",function(a){e.errorHandler&&e.errorHandler(a)}),e.addEventListener("delete-complete",function(a){e.completeHandler&&e.completeHandler(a)});var f=a.querySelector("x-app");f.go()})}(wrap(document));