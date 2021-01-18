import React, { useState } from 'react';
import { useHistory } from "react-router-dom";
import ExternalAuthenticationMethods from "../Components/ExternalAuthenticationMethods";


async function loginUser(credentials) {
  return fetch('/api/admin/KenticoAccount/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(credentials)
  })
    .then(function (response) {
      if (!response.ok) {
        throw new Error("LoginUser error: HTTP " + response.status);
      } else {
        return true;
      }
    })
    .catch(function (error) {
      alert(error);
    });
}

export default function Login() {

  const [username, setUsername] = useState('');
  const handleChangeUsername = (e) => setUsername(e.target.value);
  const [password, setPassword] = useState('');
  const handleChangePassword = (e) => setPassword(e.target.value);
  const history = useHistory();

  const handleSubmit = async (e) => {
    e.preventDefault();
    const successful = await loginUser({
      userName: username,
      password: password
    });

    if (successful) {
      history.push("/admin");
    }
  }

  return (
    <div>
      <h2>Login</h2>

      <div className="authMethods">
        <form onSubmit={handleSubmit}>
          <strong>Individual user account</strong>
          <label>
            <p>Username</p>
            <input type="text" id="usernameReact" onChange={handleChangeUsername} />
          </label>
          <label>
            <p>Password</p>
            <input type="password" id="userPasswordReact" onChange={handleChangePassword} />
          </label>
          <div>
            <button type="submit">Submit</button>
          </div>
        </form>
        <div className="otherAuthMethods">
          <strong>External authentication</strong>
          <ExternalAuthenticationMethods />
        </div>
      </div>
    </div>
  );
}

