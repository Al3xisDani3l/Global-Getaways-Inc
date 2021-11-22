const { json } = require("node:stream/consumers");


function Like(packageId, userId) {

   

    var url = "likes/like/";
        /*+ packageId + "/" + userId;*/

    var data = new FormData();
    data.append('packageId', packageId);
    data.append('userId', userId);

    fetch(url, {
        method: 'post',
        credentials: 'same-origin',
        body: data
       



    }).then(es => es.json())
        .then(j => console.log(j))
        .catch(error => console.error('Error', error));
}

function Dislike(packageId, userId) {

    var url = "likes/dislike/";
    /*+ packageId + "/" + userId;*/

    var data = new FormData();
    data.append('packageId', packageId);
    data.append('userId', userId);

    fetch(url, {
        method: 'post',
        credentials: 'same-origin',
        body: data




    }).then(es => es.json())
        .then(j => console.log(j))
        .catch(error => console.error('Error', error));
}

function AddTravel(packageId, userId) {

    var url = "Shopping/addTravel/";
    /*+ packageId + "/" + userId;*/

    var data = new FormData();
    data.append('packageId', packageId);
    data.append('userId', userId);

    fetch(url, {
        method: 'post',
        credentials: 'same-origin',
        body: data




    }).then(es => es.json())
        .then(j => console.log(j))
        .catch(error => console.error('Error', error));
}

function RemoveTravel(packageId, userId) {

    var url = "Shopping/removeTravel/";
    /*+ packageId + "/" + userId;*/

    var data = new FormData();
    data.append('packageId', packageId);
    data.append('userId', userId);

    fetch(url, {
        method: 'post',
        credentials: 'same-origin',
        body: data




    }).then(es => es.json())
        .then(j => console.log(j))
        .catch(error => console.error('Error', error));
}

function RemoveAllTravel(packageId, userId) {

    var url = "Shopping/removeAllTravel/";
    /*+ packageId + "/" + userId;*/

    var data = new FormData();
    data.append('packageId', packageId);
    data.append('userId', userId);

    fetch(url, {
        method: 'post',
        credentials: 'same-origin',
        body: data




    }).then(es => es.json())
        .then(j => console.log(j))
        .catch(error => console.error('Error', error));
}


function UpdateDown(cartId, packageId, userId) {

    var contador = document.getElementById("contador"+cartId);

    if (parseInt(contador.value) - 1 == 0) {
        RemoveAllTravel(packageId, userId);
        var carItem = document.getElementById("carItem" + cartId);
        carItem.style.display = 'none';
    }
    else {
        RemoveTravel(packageId, userId);
        contador.value = parserInt(contador.value) - 1;
    }

}

function UpdateUp(cartId,packageId, userId) {

    var contador = document.getElementById("contador" + cartId);

    
        AddTravel(packageId, userId);
     contador.value = parserInt(contador.value) + 1;

    

}

function UpdateDelete(cartId, packageId, userId) {

    RemoveAllTravel(packageId, userId);
    var carItem = document.getElementById("carItem" + cartId);
    carItem.style.display = 'none';



}


function postFunction(url) {

    fetch(url, {
        method: 'post',
        credentials: 'include',
        headers: {
            'Accept': 'application/json;charset=utf-8',
            'Content-type': 'application/json;charset=UTF-8'
        }



    }).then(es => es.json())
        .then(j => console.log(j))
        .catch(error => console.error('Error', error));
}

