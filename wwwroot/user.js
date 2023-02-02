const userUri = '/User';
let users = [];
function getUsers() {
    debugger
    fetch("/User", {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage.getItem("token")
        }
    })
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.log(alert('you dont have authorize')));
}
const addUser = () => {
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');

    const user = {
        id:0,
        name: addNameTextbox.value.trim(),
        password: addPasswordTextbox.value.trim()

    };

    fetch(`${userUri}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            },
            body: JSON.stringify(user)
        })
        .then(() => {
            getUsers();
            addNameTextbox.innerHTML = '';
            addPasswordTextbox.innerHTML = '';
        })
        .catch(error => console.error('Unable to add user.', error));
}
const deleteUser = (id) => {
    debugger
    fetch(`${userUri}/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            }
        })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete user.', error));
}


const _displayCount = (userCount) => {
    const name = (userCount === 1) ? 'user' : 'users kinds';
    document.getElementById('counter').innerText = `${userCount} ${name}`;
}

const _displayUsers = (data) => {
    debugger
    const tBody = document.getElementById('users');
    tBody.innerHTML = '';
    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(user => {

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(user.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNode2 = document.createTextNode(user.password);
        td2.appendChild(textNode2);

        let td3 = tr.insertCell(2);
        td3.appendChild(deleteButton);
    });

    users = data;
}