var url='http://localhost:58152/';
var id;

$(document).ready(function () {
    var querystring = new URLSearchParams(window.location.search);
    
    if (querystring.has('ID')) {
        id = parseInt(querystring.get('ID'));
        if (id > 0) {
            cargarUsuario(id);
        }
    }

});

function cargarUsuario(ID)
{
    cargarDatosUsuario(ID);
    cargarFotosUsuario(ID);
}
function cargarDatosUsuario(ID)
{
    var urlAPI=url+'/api/personas/'+ID;
    

    var settings = {
        "url": urlAPI,
        "method": "GET",
        "timeout": 0,
      };
      
      $.ajax(settings).done(function (response) {
        var txtID= document.getElementById('txt_id');
        var txtNombre= document.getElementById('txt_nombre');
        txtID.value= response.id;
        txtNombre.value=response.nombre;
      });


    // sendRequest(HTTPVerbs.get,urlAPI,true,contentType.jsonUTF8,responseType.jsonp,false,JSON.stringify(data),
    //             function success(result) {
                    
    //                 var txtID= document.getElementById('txt_id');
    //                 var txtNombre= document.getElementById('txt_nombre');
    //                 txtID.value= result.id;
    //                 txtNombre.value=result.nombre;
    //             },
    //             function error(result) {

    //             },
    //             function complete() {

    //             });
}

function cargarFotosUsuario(ID)
{
    var urlAPI=url+'/api/ImagenesPersonas/fotos/'+ID;

    var settings = {
        "url": urlAPI,
        "method": "GET",
        "timeout": 0,
      };
      
      $.ajax(settings).done(function (response) {
        var _html='';
        var _html2='';
        var lstImagenes= document.getElementById('listaImagenes');
        var lstImagenesTrain=document.getElementById('listaImagenesEntrenamiento');
        if (response.length>0)
        {
            for (var n=0;n<response.length;n++)
            {
                _html+='<div class="col-md-3">'
                _html+='<img src="'+"data:image/png;base64,"+ response[n].fotocompleta +'" alt="IMG" width="256" height="256" srcset="" style="width:100%;height:100%;">'
                _html+='<label>'+response[n].fecha+'</label>'
                _html+='</div>'

                _html2+='<div class="col-md-1">'
                _html2+='<img src="'+"data:image/png;base64,"+ response[n].foto +'" alt="IMG" width="64" height="64" srcset="" style="width:100%;height:100%;">'
                _html2+='<label>'+response[n].fecha+'</label>'
                _html2+='</div>'
            }
        }
        else{
            _html+='<div class="col-md-12 text-center">'                    
            _html+='<label>El usuario no posee imagenes cargadas</label>'
            _html+='</div>'
            _html2+='<div class="col-md-12 text-center">'                    
            _html2+='<label>El usuario no posee imagenes cargadas</label>'
            _html2+='</div>'
        }
        lstImagenes.innerHTML=_html;
        lstImagenesTrain.innerHTML=_html2;
      });
      

    // sendRequest(HTTPVerbs.get,urlAPI,true,contentType.jsonUTF8,responseType.jsonp,false,JSON.stringify(data),
    //             function success(result) {
    //                 var _html='';
    //                 var lstImagenes= document.getElementById('listaImagenes');
    //                 for (var n=0;n<result.length;n++)
    //                 {
    //                     _html+='<div class="col-md-3">'
    //                     _html+='<img src="data:image/png;base64,'+ result.foto +'" alt="IMG" width="256" height="256" srcset="">'
    //                     _html+='<label>'+result.fecha+'</label>'
    //                     _html+='</div>'
    //                 }
    //                 lstImagenes.innerHTML=_html;
                    
    //             },
    //             function error(result) {
    //                 var lstImagenes= document.getElementById('listaImagenes');
    //                 var _html='';
    //                 _html+='<div class="col-md-12 text-center">'                    
    //                 _html+='<label>El usuario no posee imagenes cargadas</label>'
    //                 _html+='</div>'
    //                 lstImagenes.innerHTML=_html;
    //             },
    //             function complete() {

    //             });
}

function subirFotoUsuario(){
    var urlAPI=url+'/api/ImagenesPersonas/';
    var img64=document.getElementById('photo').src.replaceAll("data:image/png;base64,","");

    //var data='{"ID":"1","foto":"'+img64 +'","fecha":"2020/01/01","personaID":"'+id+'"}';
    var DA={'personaID':id,'fotocompleta': img64};

    var settings = {
        "url": urlAPI,
        "method": "POST",
        "timeout": 0,
        "headers": {
          "Content-Type": "application/json"

        },
        "data": JSON.stringify(DA),
      };
      
     

      $.ajax(settings).done(function (response) {
        cargarFotosUsuario(id)
      });
     

    // sendRequest(HTTPVerbs.post,urlAPI,true,contentType.json,responseType.jsonp,false,JSON.stringify(data),
    //             function success(result) {
                    
    //                 var txtID= document.getElementById('txt_id');
    //                 var txtNombre= document.getElementById('txt_nombre');
    //                 txtID.value= result.ID;
    //                 txtNombre.value=result.nombre;
    //             },
    //             function error(result) {

    //             },
    //             function complete() {
    //                 cargarFotosUsuario(id);
    //             });
}
