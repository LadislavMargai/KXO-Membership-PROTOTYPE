import React, { useState, useEffect } from 'react';
import { useHistory } from "react-router-dom";

const AuthenticatedUserDetails = function () {
  const [user, setUser] = useState(null);
  const history = useHistory();

  useEffect(() => {
    fetch('/api/admin/KenticoAccount/MyProfile')
      .then((response) => {
        if (response.status === 400) {
          // Temp solution - server vrati 400 pro neautorizovany requesty .. aza vrati 401, tak muzeme odstranit
          history.push('/admin/login');
          return Promise.reject();
        }
        if (response.status === 401) {
          console.log("Not authenticated -> Redirecting to Login page. HTTP Response status: " + response.status);
          history.push('/admin/login');
          return Promise.reject();
        }
        else if (!response.ok) {
          return Promise.reject("HTTP Error:" + response.status);
        }
        return response.json();
      })
      .then(
        // Success
        (user) => setUser(user),
        // Error
        (error) => { if (error) { alert(error); }}
      )

    // React needs to cleanup the useEffect for the login redirect
    return () => { };
  }, []);


  if (!user) {
    return <div>User data loading ...</div>
  }

  return (
    <div>
      {
        Object.keys(user).map((key) => { return <div key={key}><span className="label" >{key}:</span><span>{user[key]}</span></div> })
      }
    </div>
  );
}


export default function Dashboard() {
  return (
    <div>
      <h2>Dashboard</h2>
      <h4>Authenticated user</h4>
      <AuthenticatedUserDetails />
    </div>
  );
}