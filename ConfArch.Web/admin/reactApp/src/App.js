import {
  BrowserRouter as Router,
  Switch,
  Route,
  NavLink
} from "react-router-dom";
import Login from "./Pages/login";
import Dashboard from "./Pages/dashboard";
import React from 'react';
import Logout from "./Components/Logout";

function App() {
  return (
    <Router>
      <div>
        <div className="nav">
          <ul>
            <li>
              <NavLink to="/admin" exact activeClassName="selected">Dashboard</NavLink>
            </li>
            <li>
              <NavLink to="/admin/login" exact activeClassName="selected">Login</NavLink>
            </li>
            <li>
              <a href="/">Live site</a>
            </li>
            <li>
              <Logout />
            </li>

          </ul>
        </div>
        <hr />
        <div>
          <Switch>
            <Route exact path="/admin/">
              <Dashboard />
            </Route>
            <Route exact path="/admin/login">
              <Login />
            </Route>

            {/* Problem: Nedari se mi excludovat podstrom z React routeru: "/admin/api/login" - porad se chyta router */}
          </Switch>
        </div>
      </div>
    </Router>
  );
}

export default App;
