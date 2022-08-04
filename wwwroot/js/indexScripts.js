window.onload = (event) => renderUsers();
document.getElementById("ban").addEventListener("click", () => DoPostAction("ban"));
document.getElementById("unban").addEventListener("click", () => DoPostAction("unban"));
document.getElementById("del").addEventListener("click", () => DoPostAction("del"));

async function getUsers() {
    let url = 'account/admin';
    try {
        let res = await fetch(url);
        return await res.json();
    } catch (error) {
        console.log(error);
    }
}

async function renderUsers() {
    let users = await getUsers();
    let html = '<tr>\n' +
        '                 <th><input type="checkbox" id="CBAll" onclick="CheckedAll()"></th>\n' +
        '                 <th>User name</th>\n' +
        '                 <th>Email</th>\n' +
        '                 <th>Registration time</th>\n' +
        '                 <th>Last login</th>\n' +
        '             </tr>';
    if (users.length > 0){
    users.forEach(user => {
        let colorPropery = '';
        if (user.isBanned) colorPropery = 'style="color:red"'
        let htmlSegment = `
                    <tr ${colorPropery}>
                        <th><input type="checkbox" name="CB" id=@user.UserName/></th>
                        <td>${user.Name}</td>
                        <td>${user.Email}</td>
                        <td>${user.LastLogin}</td>
                        <td>${user.RegistrationTime}</td>
                    </tr>
                `;
        html += htmlSegment;
    });
    } else
        {
            html += "<td colspan=5>There are no users</td>>"
        }
    let container = document.querySelector('.table-striped');
    container.innerHTML = html;
}

function CheckedAll()
{
    let CBAll = document.getElementById("CBAll")
    let checkboxes = document.getElementsByName("CB")
        //.forEach(C => { console.log(c.id); c.checked = CBAll.checked; })
    for (let cb of checkboxes){
        cb.checked = CBAll.checked
    }
}

function DoPostAction(Act){
    let checkboxes = document.getElementsByName("CB")
    let checkedNames = []
    for (let cb of checkboxes){
        if (cb.checked){
            checkedNames.push(cb.id.substring(0, cb.id.length - 1))
        }
    }
    
    $.ajax({
        type: "POST",
        data :JSON.stringify(checkedNames),
        url: "admin/" + Act,
        contentType: "application/json",
        error: function (jqXHR, exception) {$('post').html( exception ); }
    });
}
