window.onload = function() {
 var btns = document.querySelectorAll('[data-redir]');
 for(i = 0; i < btns.length; i++) {
     var btn = btns[i];
     
     btn.onclick = function() {
        window.location = this.getAttribute("data-redir");
        return false;
     };
 }
};
