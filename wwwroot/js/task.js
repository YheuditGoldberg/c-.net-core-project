const uri = '/Task';
var taskas=[];
function getItems(token) {
    var myHeaders = new Headers();
    myHeaders.append("Authorization", "Bearer " + token);
    myHeaders.append("Content-Type", "application/json");
    var requestOptions = {
        method: 'GET',
        headers: myHeaders,
        redirect: 'follow'
    };

    fetch("/Task", requestOptions)
        .then(response => response.json())
        .then(result=>{_displayItems(result);taskas=result})
        .catch(error => console.log('error', error));
}

function deleteItem(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + sessionStorage.getItem("token")
        },
    })
        .then(() => getItems(sessionStorage.getItem("token")))
        .catch(error => console.log('Unable to delete item.', error));
}

function addItem() {
    debugger
    const addNameTextbox = document.getElementById('add-name');
    var myHeaders = new Headers();

    myHeaders.append("Content-Type", "application/json");
    var raw = JSON.stringify({
        Id: 0,
        IdTask:0,
        Name:  addNameTextbox.value.trim(),
        IsDone: false,
            })
    var requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow",
    };

    fetch("https://localhost:7130/Task", requestOptions)
        .then((response) => response.text())
        .then(() => {
            const div=document.createElement("div");
            document.getElementById('container').replaceChildren(div);
    
            getItems(sessionStorage.getItem("token"));
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));

}

const displayUpdateForm = (id) => {
    const item = items.find(item => item.idTask == id);
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.idTask;
    document.getElementById('edit-isDo').checked=item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

const updateItem = () => {
   
    
    const itemId = document.getElementById('edit-id').value;
    const iduser = taskas.find(item => item.idTask == itemId);
    const item = {
        Id:iduser.id,
        IdTask:itemId,
        Name: document.getElementById('edit-name').value.trim(),
        IsDone: document.getElementById('edit-isDo').checked,
    };

    fetch(`${uri}/${item.IdTask}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            },
            body: JSON.stringify(item)
        })
        .then(() => getItems(sessionStorage.getItem("token")))
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

const closeInput = () => {
    document.getElementById('editForm').style.display = 'none';
}

const _displayCount = (itemCount) => {
    const name = (itemCount === 1) ? 'task' : 'tasks kinds';

    document.getElementById('counter').innerText = `${itemCount} ${name}`;
}
const _displayItems = (data) => {
    const tBody = document.getElementById('items');
    tBody.innerHTML = '';
    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoCheckbox = document.createElement('input');
        isDoCheckbox.type = 'checkbox';
        isDoCheckbox.disabled = true;
        isDoCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Update';
        editButton.setAttribute('onclick', `displayUpdateForm(${item.idTask})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.idTask})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);
        

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    items = data;
}

