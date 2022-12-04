import getConfig from "next/config";

import Layout from "@/components/Layout";
import { useEffect, useState } from "react";
import { LeaderboardEntry, Score } from "src/types";

const { publicRuntimeConfig } = getConfig();
const { name } = publicRuntimeConfig.site;

const Home = () => {
  const [level, setLevel] = useState(0);
  const [leaderboardData, setLeaderboardData] = useState({});

  const buildLeaderboard = (level: number) => {
    const children = [];
    for (const [key, value] of Object.entries(leaderboardData)) {
      const entry = value as Score;
      if (entry.level === level) {
        children.push(
          <div className="flex flex-row justify-between border-b-2 border-gray-200">
            <span>{key}</span>
            <span>{entry.time}</span>
          </div>
        );
      }
    }
    return children;
  };

  useEffect(() => {
    fetch("http://localhost:3000/api/leaderboard").then((res) => {
      res.json().then((data) => setLeaderboardData(data));
    });
  }, []);

  return (
    <Layout>
      <div className="w-full h-screen | flex flex-col items-center | p-2 | bg-slate-900 text-white">
        <h1 className="text-6xl my-10">Freshaliens Leaderboard</h1>
        <div className="flex flex-row">
          <button
            className={
              "p-2 text-yellow-400 border-2 border-current rounded-md" +
              (level === 0 ? " text-black bg-yellow-400" : "")
            }
            type="button"
            onClick={() => setLevel(0)}
          >
            Level 1
          </button>
          <button
            className={
              "p-2 text-yellow-400 border-2 border-current rounded-md" +
              (level === 1 ? " text-black bg-yellow-400" : "")
            }
            type="button"
            onClick={() => setLevel(1)}
          >
            Level 2
          </button>
          <button
            className={
              "p-2 text-yellow-400 border-2 border-current rounded-md" +
              (level === 2 ? " text-black bg-yellow-400" : "")
            }
            type="button"
            onClick={() => setLevel(2)}
          >
            Level 3
          </button>
        </div>
        <div className="w-[50%] mt-5 border-2 p-10">
          <div className="flex flex-row justify-between text-yellow-400 mb-2">
            <span>Name</span>
            <span>Time</span>
          </div>

          {buildLeaderboard(level + 1)}
        </div>
      </div>
    </Layout>
  );
};

export default Home;
