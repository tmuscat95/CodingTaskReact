import React, { useContext, useState } from "react";
import { AuthContext } from "./AuthProvider";
import { Form, Button, FormGroup, Label, Input } from "reactstrap";
import { useHistory } from "react-router";
function Login(props) {
  const { authHeader, doLogin, loggedIn } = useContext(AuthContext);
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const history = useHistory();
  const onChangeUsername = (e) => {
    setUsername(e.target.value);
  };

  const onChangePassword = (e) => {
    setPassword(e.target.value);
  };

  const onSubmitHandler = async () => {
      const success = await doLogin(username, password);
      if (success) {
          history.push("/");
          //alert("Success");
      }
      else {
          alert("Failure.");
      }
      
    };


  return (
    <div>
      <Form
        onSubmit={(e) => {
          e.preventDefault();
          onSubmitHandler();
        }}
      >
        <FormGroup>
          <Label for="username">Username</Label>
          <Input
            onChange={onChangeUsername}
            value={username}
            id="username"
            name="username"
            placeholder="username"
            type="text"
          />
        </FormGroup>

        <FormGroup>
          <Label for="password">Password</Label>
          <Input
            onChange={onChangePassword}
            value={password}
            id="password"
            name="password"
            placeholder="password"
            type="password"
          />
        </FormGroup>
        <Button variant="primary" type="submit">
          Log In
        </Button>
      </Form>
    </div>
  );
}

export default Login;
