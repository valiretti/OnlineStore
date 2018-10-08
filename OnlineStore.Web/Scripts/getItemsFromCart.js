


function getCart() {
    return localStorage.getItem('cart');
}

function getCartItems() {
    let itemsId = getCart();
    var request = new XMLHttpRequest;
    
    request.onreadystatechange = reqReadyStateChange;
    function reqReadyStateChange() {
        if (request.readyState === 4) {
            var status = request.status;
            if (status === 200) {
                var cartContent = JSON.parse(request.responseText);
                var strResult = "<table><th>Название</th><th>Описание товара</th><th>Цена</th>";
                for (var item of cartContent) {
                    strResult += "<tr><td>" + item.Name + "</td><td> " + item.PhoneDescription + "</td><td>" +
                        item.Price + "</td><td>";
                }
                strResult += "</table>";
                document.getElementById('cart_content').innerHTML = strResult;
            }
        }
    }
    request.open("GET", "/home/GetPhonesForCart?json=" + itemsId);
    request.send();
}


document.onreadystatechange = function () {
    if (document.readyState === "interactive") {
        getCartItems();
    }
}