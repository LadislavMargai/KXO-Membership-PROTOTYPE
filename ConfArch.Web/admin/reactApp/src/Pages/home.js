import React from 'react';

export default function Home({ user }) {

  function AuthenticatedUser({user}) {
    return <table><tbody>
      <tr>
        <td>Name:</td>
        <td>{user.name}</td>
      </tr>
      <tr>
        <td>Guid:</td>
        <td>{user.guid}</td>
      </tr>
      <tr>
        <td>Email:</td>
        <td>{user.email}</td>
      </tr>
      <tr>
        <td>Is admin:</td>
        <td>{user.isAdmin ? "true" : "false"}</td>
      </tr>
      </tbody></table>
  }

  function UnknownUserInformation() {
    return <div>Public</div>
  }

  function UserInformation({user}) {
    if (user) {
      return <AuthenticatedUser user={user} />
    }
    else {
      return <UnknownUserInformation />
    }
  }

  return (
    <div>
      <h2>Home</h2>
      <h4>Authenticated user</h4>
      <UserInformation user={user} />
    </div>
  );
}