import React, { useState } from "react";
import axios from "axios";

export const AuthContext = React.createContext();

function AuthProvider({ children }) {
  const [loggedIn,setLoggedIn] = useState(false);
  const [loggedInCreds, setLoggedInCreds] = useState({});



  const doLogin = async (username, password) => {
    const res = await axios.post("api/login", { username: username, password: password });
    

    if(res.status===200){
        setLoggedIn(true);
        setLoggedInCreds({ username: username, password: password });
        return true;
    } else
        return false;
  };
  const authHeader = () =>{
    
    return { 'Authorization': 'Basic ' + btoa(loggedInCreds.username + ":" + loggedInCreds.password)};
  }

    const value = { authHeader, doLogin, loggedIn }
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export default AuthProvider;
