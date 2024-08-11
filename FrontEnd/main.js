let Token = ''

axios.post('', {
    UserName: 'a',
    Password: 'a'
}).then((response) => {
    Token = response.data.token
    console.log(Token)
})
