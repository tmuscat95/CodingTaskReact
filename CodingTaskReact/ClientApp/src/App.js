import React, { Component } from "react";
import { Route } from "react-router-dom";
import { Layout } from "./components/Layout";
import { Home } from "./components/Home";
import { FetchData } from "./components/FetchData";
import { Counter } from "./components/Counter";

import "./custom.css";
import Login from "./components/Login";
import AuthProvider from "./components/AuthProvider";

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
      <AuthProvider>
        <Layout>
          <Route exact path="/" component={FetchData} />
          <Route exact path="/login" component={Login} />
        </Layout>
      </AuthProvider>
    );
  }
}
