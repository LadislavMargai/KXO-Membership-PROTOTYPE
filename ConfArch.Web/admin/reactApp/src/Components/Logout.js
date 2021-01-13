import { useHistory } from "react-router-dom";

export default function Logout() {
    const history = useHistory();

    const logout = (e) => {

        e.preventDefault()

        fetch('/api/admin/KenticoAccount/logout', {
            method: 'POST',
        })
            .then(function (response) {
                // Do not report 302 redirects after logout
                if (!response.ok && !response.redirected) {
                    throw new Error("Logout error: HTTP " + response.status);
                }
                history.push('/admin/login');
            })
            .catch(function (error) {
                alert(error);
            })

    }

    return (
        <a href="" onClick={logout} >Logout</a>
    );
}

