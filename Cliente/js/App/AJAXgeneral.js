var HTTPVerbs = {
    post: 'POST',
    get: 'GET'
};
var contentType = {
    json: 'application/json',
    jsonUTF8: 'application/json; charset=utf-8'
}

var responseType = {
    jsonp:'jsonp',
    json: 'json',
    xml: 'xml',
    html: 'html',
    js: 'script',
    txt: 'text'
}

var responseStatus = {
    err: 0,
    warning: 1,
    ok: 2
}

function sendRequest(verb, url, sync,contentType, responseType, cache, data, success, error, complete) {
    $.ajax({
        
        url: url,
        type: verb,

        // dataType: responseType,
        // async: sync,
        // cache: cache,
        // data: data,
        // crossDomain:true,
        // contentType: contentType,
        error: function (xhr, status, result) {
            if (error != null) {
                error(result);
            }
        },
        success: function (result) {
            if (success != null) {
                success(result);
            }
        },
        complete: function () {
            if (complete != null) {
                complete();
            }
        }
    });
}

function sendRequest2(verb, url, sync,contentType, responseType, cache, data, success, error, complete) {
    $.ajax({
        
        url: url,
        type: verb,

         dataType: responseType,
        // async: sync,
        // cache: cache,
        // data: data,
        // crossDomain:true,
        // contentType: contentType,
        error: function (xhr, status, result) {
            if (error != null) {
                error(result);
            }
        },
        success: function (result) {
            if (success != null) {
                success(result);
            }
        },
        complete: function () {
            if (complete != null) {
                complete();
            }
        }
    });
}

