// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function checkRePassword() {
    const password = document.getElementById("password");
    const reEnter = document.getElementById("reEnter");
    const registerBtn = document.getElementsByClassName("register-btn")[0];

    if (password.value !== reEnter.value) {
        document.getElementsByClassName("err-re-enter")[0].style.display = "inline-block";
        reEnter.classList.add("border-red");
        registerBtn.disabled = true;
    } else {
        document.getElementsByClassName("err-re-enter")[0].style.display = "none";
        reEnter.classList.remove("border-red");
        registerBtn.disabled = false;
    }
}
var listName = document.getElementsByClassName('js-formatInputName')[0];
var wrapCompose = document.getElementsByClassName('wrap-compose')[0];
var NameReceiver = document.getElementsByClassName('nameReceiver');
listName.addEventListener('keydown', (e, v) => {
    var userNameHtml = document.createElement("span");
    var textnode = document.createTextNode(listName.value);
    userNameHtml.appendChild(textnode);
    userNameHtml.classList.add('nameReceiver');

    if (e.keyCode == 32 || e.keyCode == 13 || e.keyCode == 9 || e.w) {

        wrapCompose.insertBefore(userNameHtml, wrapCompose.childNodes[NameReceiver.length + 1]);
        listName.value = '';
        console.log(NameReceiver);
        e.preventDefault();
    } else if (e.keyCode == 8) {
        wrapCompose.removeChild(wrapCompose.childNodes[NameReceiver.length]);
    }
})
listName.addEventListener('keydown', (e) => {
    if (e.keyCode == 9) {
        e.preventDefault();
    }

})
var btnSendEmail = document.getElementById('sendEmail');
btnSendEmail.addEventListener('click', () => {


    var listNameUser = '';
    for (let i = 0; i < NameReceiver.length; i++) {

        listNameUser += NameReceiver[i].innerText + ',';

    }

    document.getElementById('reciverName').value = listNameUser.substring(0, listNameUser.length - 1);




})
