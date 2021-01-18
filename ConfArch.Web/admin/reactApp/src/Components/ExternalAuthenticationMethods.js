import React, { useState, useEffect } from 'react';

async function getAuthMethods(setAuthMethods) {
  return fetch('/api/admin/KenticoAccount/ExternalAuthenticationMethods')
    .then((response) => {
      if (!response.ok) {
        return Promise.reject("HTTP Error:" + response.status);
      }
      return response.json();
    })
    .then(
      // Success
      (authMethods) => { setAuthMethods(authMethods); console.log(authMethods); authMethods.forEach((m) => { console.log(m); })},
      // Error
      (error) => { if (error) { alert(error); } }
    );
}

export default function ExternalAuthenticationMethods() {
  const [authMethods, setAuthMethods] = useState([]);

  useEffect(() => {
    getAuthMethods(setAuthMethods);

    // React needs to cleanup the useEffect for the login redirect
    return () => { };
  }, []);

  if (!authMethods.length) {
    return <div>Authentication methods loading ...</div>
  }  

  return (
    <div>
      {
        authMethods.map((m) => { return <div><a href={m.url} >{m.name}</a></div> })
      }
    </div>
  );
}