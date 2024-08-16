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



async function AddPostToHtml(post) {
    let s1 = `                <div class="card shadow mb-4">
                    <div class="card-header">
                        <img class="rounded-circle border border-2 " style="height: 40px; width: 40px;" src="Pics/2.jpg"
                            alt="??">
                        <b class="ms-2">@gg</b>
                    </div>
                    <div class="card-body">
                        <img style="max-height: 330px; object-fit: cover;" class="w-100" src="Pics/${post.image ?? "0.png"}" alt="">
                        <h6 class="mt-1" style="color: grey;">${GitDateDiffrence(post.createdAt)}</h6>
                        <h5 style="font-weight: bold;">${post.title}</h5>
                        <p>${post.body}</p>
                        <hr>
                        <div class="footer">
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

function GetUserById(id) {
    axios.get(`${Url}/Users/${1}`)
        .then((response) => {
            return response.data
        })
}


function btnLoginClicked() {
    let UserName = document.getElementById("username-input").value
    let Password = document.getElementById("password-input").value

    const params = {
        "username": UserName,
        "password": Password
    }

    axios.post(`${Url}/Auth/Login`, params).then((response) => {
        console.log(response.data)
    }).catch((error) => {
        console.log(error)
    })

}
