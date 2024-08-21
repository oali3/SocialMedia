let Url = 'https://localhost:7179/api'
let Token = ''
let Posts = []

axios.get(`${Url}/Posts`).then((response) => {
    Posts = response.data
    for (Post of Posts) {
        AddPostToHtml(Post)
    }
}).catch((error) => {
    document.getElementById("Post-Container").innerHTML = `<h1>No Posts</h1>`
})

setUpUI()


async function AddPostToHtml(post) {
    let s1 = `                <div class="card shadow mb-4">
                    <div class="card-header">
                        <img class="rounded-circle border border-2 " style="height: 40px; width: 40px;" src="Pics/${post.aothor.image ?? "0.png"}"
                            alt="??">
                        <b class="ms-2">@${post.aothor.userName}</b>
                    </div>
                    <div class="card-body p-0">
                        <img style="max-height: 330px; object-fit: cover;" class="w-100" src="Pics/${post.image ?? "0.png"}" alt="">
                        <h6 class="ms-3 mt-1" style="color: grey;">${GitDateDiffrence(post.createdAt)}</h6>
                        <h5 class="ms-3" style="font-weight: bold;">${post.title}</h5>
                        <p class="ms-3">${post.body}</p>
                        <hr>
                        <div class="footer ms-3 mb-3">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                                class="bi bi-pen" viewBox="0 0 16 16">
                                <path
                                    d="m13.498.795.149-.149a1.207 1.207 0 1 1 1.707 1.708l-.149.148a1.5 1.5 0 0 1-.059 2.059L4.854 14.854a.5.5 0 0 1-.233.131l-4 1a.5.5 0 0 1-.606-.606l1-4a.5.5 0 0 1 .131-.232l9.642-9.642a.5.5 0 0 0-.642.056L6.854 4.854a.5.5 0 1 1-.708-.708L9.44.854A1.5 1.5 0 0 1 11.5.796a1.5 1.5 0 0 1 1.998-.001m-.644.766a.5.5 0 0 0-.707 0L1.95 11.756l-.764 3.057 3.057-.764L14.44 3.854a.5.5 0 0 0 0-.708z" />
                            </svg>
                            <span class="ms-1">(3) Comments</span>
                        </div>
                    </div>
                </div>`

    document.getElementById("Post-Container").innerHTML += s1
}

function GitDateDiffrence(date) {
    const Now = new Date()
    const GivenDate = new Date(date)

    const diffInM = Math.floor((Now - GivenDate) / 60000);

    if (diffInM < 60)
        return `${diffInM} min ago`
    else if (diffInM < 1440)
        return `${Math.floor(diffInM / 60)} hour ago`
    else
        return `${Math.floor(diffInM / 1440)} day ago`
}





function setUpUI() {
    let token = localStorage.getItem("token")
    let user = JSON.parse(localStorage.getItem("user"))

    const divLogin = document.getElementById("div-login")
    const btnlogout = document.getElementById("div-logout")
    const headerUsername = document.getElementById("header-username")
    const headerPicture = document.getElementById("header-picture")
    const btnAddPost = document.getElementById("add-post-btn")


    if (token == null) {
        divLogin.style.setProperty("display", "flex")
        btnlogout.style.setProperty("display", "none", "important")

        headerUsername.parentElement.classList.remove('d-flex')
        btnAddPost.classList.add('d-none')
    } else {
        divLogin.style.setProperty("display", "none", "important")
        btnlogout.style.setProperty("display", "flex")

        headerUsername.innerHTML = '@' + user.userName
        headerPicture.setAttribute('src', `Pics/${user.image ?? '0.png'}`)

        headerUsername.parentElement.classList.add('d-flex')
        btnAddPost.classList.remove('d-none')

    }
}
function btnLoginClicked() {
    let UserName = document.getElementById("username-input").value
    let Password = document.getElementById("password-input").value

    const params = {
        "username": UserName,
        "password": Password
    }

    axios.post(`${Url}/Auth/Login`, params).then((response) => {
        localStorage.setItem("token", response.data.token)
        localStorage.setItem("user", JSON.stringify(response.data.user))
        bootstrap.Modal.getInstance(document.getElementById("login-modal")).hide()
        ShowCustomAlert('Login Successfully')
        setUpUI()
    }).catch(error => {
        ShowCustomAlert(error.request.response, true)
    })
}
function btnSignUpClicked() {
    const Name = document.getElementById("name-input").value
    const Email = document.getElementById("email-input").value
    const UserName = document.getElementById("username-signup-input").value
    const Password = document.getElementById("password-signup-input").value

    const params = {
        "name": Name,
        "email": Email,
        "username": UserName,
        "password": Password
    }

    axios.post(`${Url}/Auth/Register`, params).then((response) => {
        localStorage.setItem("token", response.data.token)
        bootstrap.Modal.getInstance(document.getElementById("signup-modal")).hide()
        ShowCustomAlert('Sign Up Successfully')
        setUpUI()
    }).catch((error) => {
        console.log(error)
        ShowCustomAlert(error.response.data, true)
    })
}
function btnLogoutClicked() {
    localStorage.removeItem("token")
    localStorage.removeItem("user")
    setUpUI()
    ShowCustomAlert('Logout Successfully')
}
function ShowCustomAlert(message, isDanger = false) {
    const alertPlaceholder = document.getElementById('successful-login')
    const appendAlert = (message, type) => {
        const wrapper = document.createElement('div')
        wrapper.innerHTML = [
            `<div class="alert alert-${type} alert-dismissible" role="alert">`,
            `   <div>${message}</div>`,
            '   <button id="btn-close-alert" type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>',
            '</div>'
        ].join('')

        alertPlaceholder.append(wrapper)
    }
    appendAlert(message, (isDanger ? 'danger' : 'success'))

    setTimeout(() => {
        document.getElementById('btn-close-alert').click()
    }, 2000);
}
function btnAddPostClicked() {
    const Title = document.getElementById("post-title").value
    const Body = document.getElementById("post-body").value
    const Image = document.getElementById("post-image").files[0]

    const params = {
        "title": Title,
        "body": Body,
        "image": Image.name,
        "userid": JSON.parse(localStorage.getItem("user")).id
    }

    axios.post(`${Url}/Posts`, params).then((response) => {
        bootstrap.Modal.getInstance(document.getElementById("add-post-modal")).hide()
        ShowCustomAlert('Post Added Successfully')
        setUpUI()
    }).catch((error) => {
        console.log(error)
        ShowCustomAlert(error.response.data, true)
    })
}