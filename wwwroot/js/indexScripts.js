window.onload = () => {renderUsers(); useHandlers();}

function del(){ DoPostAction('del')}
function ban(){ DoPostAction('ban')}
function unban(){DoPostAction('unban')}

function useHandlers(){
    document.getElementById("ban").addEventListener("click", ban);
    document.getElementById("unban").addEventListener("click", unban);
    document.getElementById("del").addEventListener("click", del);
}

async function getUsers() {
    let url = 'account/admin';
    try {
        let res = await fetch(url);
        return await res.json();
    } catch (error) {
        return getUsers()
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
                        <th><input type="checkbox" name="CB" id=${user.Name} /></th>
                        <td>${user.Name}</td>
                        <td>${user.Email}</td>
                        <td>${user.RegistrationTime}</td>
                        <td>${user.LastLogin}</td>
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
    for (let cb of checkboxes){
        cb.checked = CBAll.checked
    }
}

async function DoPostAction(Act){
    let checkboxes = document.getElementsByName("CB")
    let checkedNames = []
    console.log("trying to " + Act)
    for (let cb of checkboxes){
        if (cb.checked){
            checkedNames.push(cb.id)
        }
    }
    
    await $.ajax({
        type: "POST",
        data :JSON.stringify(checkedNames),
        url: "admin/" + Act,
        contentType: "application/json",
        error: function (jqXHR, exception) {$('post').html( exception ); }
    });
    
    location.reload()
}
