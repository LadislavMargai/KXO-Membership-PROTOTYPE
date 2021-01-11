import {
  BrowserRouter as Router,
  Switch,
  Route,
  Link
} from "react-router-dom";
import Login from "./Pages/login";
import Home from "./Pages/home";
import React, { useState } from 'react';

function App() {
  const [user, setUser] = useState(null);

  const storeUser = (data) => {
    setUser(data);
  }


  return (
    
    <Router>
      <div>
        {/* <h4>{user}</h4> */}

        Navigation menu:
        <ul>
          <li>
            <Link to="/admin">Home</Link>
          </li>
          <li>
            <Link to="/admin/login">Login</Link>
          </li>
          <li>
            <a href="#">Sign out</a>
          </li>

        </ul>

        <hr />

        <Switch>
          <Route exact path="/admin/">
            <Home user={user} />
          </Route>
          <Route exact path="/admin/login">
            <Login storeUser={storeUser} />
          </Route>

          {/* Problem: Nedari se mi excludovat podstrom z React routeru: "/admin/api/login" - porad se chyta router */}
        </Switch>
      </div>
    </Router>
  );
}

export default App;
