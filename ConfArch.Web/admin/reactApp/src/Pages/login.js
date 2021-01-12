import React, { useState } from 'react';
import { useHistory } from "react-router-dom";


async function loginUser(credentials) {
  return fetch('/api/admin/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(credentials)
  })
    .catch(function () {
      alert("LOGIN ERROR");
    })
    .then(data => data.json())
}

export default function Login({ storeUser }) {

  const [username, setUsername] = useState('');
  const handleChangeUsername = (e) => setUsername(e.target.value);
  const history = useHistory();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const user = await loginUser({
      userName: username,
      password: ''
    });

    storeUser(user);

    history.push("/admin");
  }

  return (
    <div>
      <h2>Login</h2>

      <form onSubmit={handleSubmit}>
        <label>
          <p>Username</p>
          <input type="text" id="usernameReact" onChange={handleChangeUsername} />
        </label>
        <label>
          <p>Password</p>
          <input type="password" />
        </label>
        <div>
          <button type="submit">Submit</button>
        </div>
      </form>
    </div>
  );
}

