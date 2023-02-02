
const u = document.getElementById('userButton');
const i = document.getElementById('itemButton');

u.style.display = 'none';
i.style.display = 'none';

function Login() {
    const name = document.getElementById('add-name');
    const password = document.getElementById('add-password');
    var myHeaders = new Headers();

    myHeaders.append("Content-Type", "application/json");
    var raw = JSON.stringify({
        Id: 0,
        Name: name.value.trim(),
        IsAdmin: false,
        Password: password.value.trim()
    })
    const a=name.value.trim();
    const b=password.value.trim();
    var requestOptions = {
        method: "POST",
        headers: myHeaders,
        body: raw,
        redirect: "follow",
    };

    fetch("https://localhost:7130/User/Login", requestOptions)
        .then((response) => response.text())
        .then((result) => {
            if (result.includes("401")) {
                u.style.display = 'none';
                i.style.display = 'none';
                name.value = "";
                password.value = "";
                alert("not exist!!")
            }
             else {
                token = result;
                sessionStorage.setItem("token", token)
                if (a === 'Admin'&& b === '123') {
                    u.style.display = 'block';
                    i.style.display = 'block';
                    
                }
                else{
                    
                     location.href="task.html";
                }

                               

            }
        }).catch((error) => alert("😁"+error));

}