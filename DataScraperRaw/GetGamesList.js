var linksTotal = [];

function GetAllLinks(doc){
    var res = Array.prototype.map.call(doc.querySelectorAll("#mw-pages a[href^=\"/index.php/\"]"), 
      p=>p.href);
    for(var i = 0; i < res.length; i++)
        linksTotal.push(res[i]);
}

function GetNextUrl(doc){
    var elm = Array.prototype.filter.call(doc.querySelectorAll("#mw-pages a[title=\"Category:Games\"]"), 
       p=>p.innerHTML == "next 200"
    );
    if(elm.length == 0) return null;
    return elm[0];
}

function NextRequest(doc){
    GetAllLinks(doc);
    var url = GetNextUrl(doc);
    if(url == null){
        console.log(linksTotal);
        return;
    }
    var xhr = new XMLHttpRequest();
    xhr.onload = ()=>{
        var div = document.createElement("div");
        div.innerHTML = xhr.responseText;
        NextRequest(div);
    }
    xhr.open("GET", url, true);
    xhr.send();
}

NextRequest(document);