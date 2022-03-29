var url='http://localhost:58152/';

$(document).ready(function () {
    

    listarUsuarios();
});

function altaUsuario(){
    var urlAPI=url+'/api/personas/';
    var nombre= document.getElementById('txt_nombre').value;
    var DA={'nombre':nombre,'trainData':''};
    

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
        listarUsuarios();
      });
    
}

function listarUsuarios()
{
    var urlAPI=url+'/api/personas/';
    var data=null;

    var settings = {
        "url": urlAPI,
        "method": "GET",
        "timeout": 0,
      };
      
      $.ajax(settings).done(function (response) {
        var lst = response;
                    var tabla= document.getElementById('tblUsuariosBody');
                    var _html='';
                    for (var n=0;n<lst.length;n++)
                    {
                        _html+='<tr>'
                        _html+='<td>'+lst[n].id+'</td>';
                        _html+='<td>'+lst[n].nombre+'</td>';
                        _html+='<td> <a href="usuario.html?ID='+lst[n].id+'">VER</a></td>';
                        _html+='</tr>'
                    }
                    tabla.innerHTML=_html;
      });



    // sendRequest(HTTPVerbs.get,urlAPI,true,contentType.jsonUTF8,responseType.jsonp,false,JSON.stringify(data),
    //             function success(result) {
    //                 var lst = result;
    //                 var tabla= document.getElementById('tblUsuariosBody');
    //                 var _html='';
    //                 for (var n=0;n<lst.length;n++)
    //                 {
    //                     _html+='<tr>'
    //                     _html+='<td>'+lst[n].id+'</td>';
    //                     _html+='<td>'+lst[n].nombre+'</td>';
    //                     _html+='<td> <a href="usuario.html?ID='+lst[n].id+'">VER</a></td>';
    //                     _html+='</tr>'
    //                 }
    //                 tabla.innerHTML=_html;
    //             },
    //             function error(result) {

    //             },
    //             function complete() {

    //             });

}