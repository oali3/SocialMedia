let Url = 'https://localhost:7179/api'
let CurrentPage = 1
let LastPage = 1
let post
let PostOpened = false


GetAllPosts()
setUpUI()
window.addEventListener("scroll", handleInfiniteScroll)





function AddComment() {

    let comment = document.getElementById("add-comment-input").value
    const params = {
        "text": comment,
        "postId": post.id,
        "userId": JSON.parse(localStorage.getItem("user")).id
    }

    axios.post(`${Url}/Comments`, params).then((response) => {
        ShowCustomAlert('Comment Added Successfully')
        OpenPost(post.id)
    }).catch(error => {
        ShowCustomAlert(error.request.response, true)
    })
}
function OpenPost(id) {
    PostOpened = true
    window.removeEventListener("scroll", handleInfiniteScroll)
    document.getElementById("Post-Container").innerHTML = ''
    GetFullPostHtml(id)
}
async function GetFullPostHtml(id) {
    axios.get(`${Url}/Posts/${id}`).then((response) => {
        post = response.data
        let Comments = ''
        for (comment of post.comments) {
            Comments += `   <div class="comment">
                                <img class="rounded-circle border border-2 " style="height: 30px; width: 30px;"
                                    src="Pics/${comment.user.image ?? '0.png'}" alt="??">
                                <b>${comment.user.userName}</b>
                                <p style="font-size: smaller;">${comment.text}</p>
                            </div>`
        }
        let s2 = ' '
        if (Comments != '') {
            s2 = `<hr>
                <div id="comments">
                    ${Comments}
                </div>`
        }

        let addCommentContent = ' '
        if (localStorage.getItem("token")) {
            addCommentContent = `<div class="add-comment d-flex m-2">
                    <input id="add-comment-input" type="text" placeholder="Add a comment" class="form-control me-2">
                    <button id="add-comment-btn" class="btn btn-outline-primary" type="button" onclick="AddComment()">Add</button>
                </div>`
        }
        let s1 = `<div class="card shadow mt-3 mb-4">
        <div class="card-header">
            <img class="rounded-circle border border-2 " style="height: 40px; width: 40px;" src="Pics/${post.aothor.image ?? '0.png'}"
                alt="??">
            <b class="ms-2">@${post.aothor.userName}</b>
        </div>
        <div class="card-body p-0">
            <img style="max-height: 330px; object-fit: cover;" class="w-100" src="Pics/${post.image ?? ''}" alt="">
            <h6 class="ms-3 mt-1" style="color: grey;">${GitDateDiffrence(post.createdAt)}</h6>
            <h5 class="ms-3" style="font-weight: bold;">${post.title}</h5>
            <p class="ms-3">${post.body}</p>
            <hr>
            <div class="footer ms-3 mb-3">
                <div>
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor"
                        class="bi bi-pen" viewBox="0 0 16 16">
                        <path
                            d="m13.498.795.149-.149a1.207 1.207 0 1 1 1.707 1.708l-.149.148a1.5 1.5 0 0 1-.059 2.059L4.854 14.854a.5.5 0 0 1-.233.131l-4 1a.5.5 0 0 1-.606-.606l1-4a.5.5 0 0 1 .131-.232l9.642-9.642a.5.5 0 0 0-.642.056L6.854 4.854a.5.5 0 1 1-.708-.708L9.44.854A1.5 1.5 0 0 1 11.5.796a1.5 1.5 0 0 1 1.998-.001m-.644.766a.5.5 0 0 0-.707 0L1.95 11.756l-.764 3.057 3.057-.764L14.44 3.854a.5.5 0 0 0 0-.708z">
                        </path>
                    </svg>
                    <span class="ms-1">(${post.commentsCount}) Comments</span>
                </div>
                ${s2}
                ${addCommentContent}
            </div>
        </div>
    </div>`

        document.getElementById("Post-Container").innerHTML = s1
    }).catch((error) => {
        ShowCustomAlert(error.message, true)
    })


}


function GetAllPosts(page = 1) {
    PostOpened = false
    axios.get(`${Url}/Posts?count=3&page=${page}`).then((response) => {
        if (page == 1)
            document.getElementById("Post-Container").innerHTML = ''
        let Posts = response.data.posts
        LastPage = response.data.lastPage
        for (Post of Posts) {
            AddPostToHtml(Post)
        }
    }).catch((error) => {
        document.getElementById("Post-Container").innerHTML = `<h1>No Posts</h1>`
    })
}
async function AddPostToHtml(post) {

    let currentUser = JSON.parse(localStorage.getItem("user"))
    let btnEdit = ''
    if (currentUser != null && post.userId == currentUser.id) {
        btnEdit = `\n<button class="btn btn-secondary" style="float: right;" onclick="btnEditPostClicked(event, '${post.title}', '${post.body}', ${post.id})" data-bs-toggle="modal"
                            data-bs-target="#edit-post-modal">edit</button>`
    }

    let s1 = `                <div class="card shadow mb-4" style="cursor: pointer;" onclick="OpenPost(${post.id})">
                    <div class="card-header">
                        <img class="rounded-circle border border-2 " style="height: 40px; width: 40px;" src="Pics/${post.aothor.image ?? "0.png"}"
                            alt="??">
                        <b class="ms-2">@${post.aothor.userName}</b>${btnEdit}
                    </div>
                    <div class="card-body p-0">
                        <img style="max-height: 330px; object-fit: cover;" class="w-100" src="Pics/${post.image ?? ""}" alt="">
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
                            <span class="ms-1">(${post.commentsCount ?? 0}) Comments</span>
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
    CurrentPage = 1
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
        if (user.image != "" && user.image != null)
            headerPicture.setAttribute('src', `Pics/${user.image}`)

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
        if (PostOpened)
            OpenPost(post.id)
        else
            GetAllPosts()
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
    const Image = document.getElementById("image-signup-input").files[0]

    const params = {
        "name": Name,
        "email": Email,
        "username": UserName,
        "password": Password,
        "image": (Image == null) ? '' : Image.name
    }

    axios.post(`${Url}/Auth/Register`, params).then((response) => {
        localStorage.setItem("token", response.data.token)
        localStorage.setItem("user", JSON.stringify(response.data.user))
        bootstrap.Modal.getInstance(document.getElementById("signup-modal")).hide()
        ShowCustomAlert('Sign Up Successfully')
        if (PostOpened)
            OpenPost(post.id)
        else
            GetAllPosts()
        setUpUI()
    }).catch((error) => {
        console.log(error)
        ShowCustomAlert(error.response.data, true)
    })
}
function btnLogoutClicked() {
    localStorage.removeItem("token")
    localStorage.removeItem("user")
    ShowCustomAlert('Logout Successfully')
    if (PostOpened)
        OpenPost(post.id)
    else
        GetAllPosts()
    setUpUI()
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
        "image": (Image == null) ? '' : Image.name,
        "userid": JSON.parse(localStorage.getItem("user")).id
    }

    axios.post(`${Url}/Posts`, params).then((response) => {
        bootstrap.Modal.getInstance(document.getElementById("add-post-modal")).hide()
        ShowCustomAlert('Post Added Successfully')
        setUpUI()
        GetAllPosts()
    }).catch((error) => {
        console.log(error)
        ShowCustomAlert(error.response.data, true)
    })
}
function handleInfiniteScroll() {
    const scrolledToBottom = Math.ceil(window.innerHeight + window.scrollY) >= document.body.offsetHeight - 1;
    if (scrolledToBottom) {
        if (CurrentPage < LastPage) {
            GetAllPosts(++CurrentPage);
        }
    }
}
function btnEditClicked(event, oldTitle, oldBody) {
    let Title = document.getElementById("edit-post-title").value
    let Body = document.getElementById("edit-post-body").value
    const Image = document.getElementById("edit-post-image").files[0]


    const params = {
        "title": Title,
        "body": Body,
        "image": (Image == null) ? '' : Image.name
    }

    axios.put(`${Url}/Posts/${post.id}`, params, {
        "headers": {
            "Authorization": `Bearer ${localStorage.getItem("token")}`
        }
    }).then((response) => {
        bootstrap.Modal.getInstance(document.getElementById("edit-post-modal")).hide()
        ShowCustomAlert('Post Updated Successfully')
        GetAllPosts()
        setUpUI()
    }).catch((error) => {
        ShowCustomAlert(error.response.data, true)
    })
}
function btnEditPostClicked(event, oldTitle, oldBody, postId) {
    event.stopPropagation();
    document.getElementById("edit-post-title").value = oldTitle
    document.getElementById("edit-post-body").value = oldBody

    axios.get(`${Url}/Posts/${postId}`).then((response) => {
        post = response.data
    }).catch((error) => {
        ShowCustomAlert(error.message, true)
    })
}