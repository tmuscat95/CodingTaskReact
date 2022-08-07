import React from "react";
import { useEffect } from "react/cjs/react.production.min";
import axios from "axios";

function HomeOld() {
  async function getMatches() {
    let m = await axios.get("/api/matches");
    setMatches(m);
  }

  useEffect(() => {
    getMatches();
  }, []);

  const [matches, setMatches] = useState([]);

  return (
    <>
      <ul>
        {matches.map((match) => (
          <li>{`${match.matchNo} ${match.expiryTime} ${match.winner}`}</li>
        ))}
      </ul>
      <div>
          <button onClick={getMatches}>Refresh Matches</button>
      </div>
    </>
  );
}

export default HomeOld;
