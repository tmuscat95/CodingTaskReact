import axios from "axios";
import React, { Component } from "react";
import { useContext } from "react";
import { AuthContext } from "./AuthProvider";

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

  componentDidMount() {
    this.populateData();
  }

  static renderMatchesTable(matches) {
    return (
      <table className="table table-striped" aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Match No</th>
            <th>Expirty Time</th>
            <th>Winner</th>
          </tr>
        </thead>
        <tbody>
          {matches.map((match) => (
            <tr key={match.matchNo}>
              <td>{match.matchNo}</td>
              <td>{match.expiryTime}</td>
              <td>{match.winner}</td>
            </tr>
          ))}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      FetchData.renderMatchesTable(this.state.matches)
    );

    return (
      <AuthContext.Consumer>
        {(context) => (
          <div>
            <h1 id="tabelLabel">Matches</h1>
            {contents}

            <button onClick={this.populateData}>Refresh</button>
                    <button onClick={this.resetMatches}>Reset Matches</button>
                    <button disabled={!context.loggedIn} onClick={()=>this.play(context.authHeader())}>
              Play Now
            </button>
          </div>
        )}
      </AuthContext.Consumer>
    );
  }

  async resetMatches() {
    await fetch("api/reset-matches");
    await this.populateData();
  }
  
    async populateData() {
      this.setState({ loading: true });
      const response = await fetch("api/matches");
      
    const data = await response.json();
      this.setState({ matches: data, loading: false });
      this.forceUpdate();
    }

    async play(authHeader) {
        try {
            const res = await axios.get("api/play", { headers: authHeader });
            const data = res.data;
            alert(`${data["guessedNumber"]}`);
        } catch (e) {
            alert(e.response.data);
        }
    }
}
