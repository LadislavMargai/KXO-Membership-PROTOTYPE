import { useHistory } from "react-router-dom";

export default function Signout({ storeUser }) {
    const history = useHistory();

    const signout = (e) => {

        e.preventDefault()

        fetch('/api/admin/signout', {
            method: 'POST',
        })
            .catch(function () {
                alert("SIGNOUT ERROR");
            })
            .then(() => {
                storeUser(null);
                history.push('/admin');
            })
    }

    return (
        <a href="" onClick={signout} >Sign out</a>
    );
}

