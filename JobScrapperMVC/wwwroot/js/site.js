// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function DownloadAlert() {
    var toast = document.getElementById("downloadToast");   // 1. actor ko dhoondo
                             // 2. usay "stage pe aao" bolo
    setTimeout(function () {
        toast.classList.add("show");                         // 3. 3 second baad "wapis jao" bolo
    }, 1000);
    setTimeout(function () {
        toast.classList.remove("show");                        // 3. 3 second baad "wapis jao" bolo
    }, 3000);
}