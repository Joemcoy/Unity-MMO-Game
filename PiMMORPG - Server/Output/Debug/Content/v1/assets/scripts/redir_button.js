window.onload = function() {
 var btns = document.querySelectorAll('[data-redir]');
 for(i = 0; i < btns.length; i++) {
     var btn = btns[i];
     var url = btn.getAttribute("data-redir");
     
     btn.onclick = function() {
        window.location = url;
        return false;
     };
 }
};
