import getConfig from "next/config";

import Layout from "@/components/Layout";
import { useEffect, useState } from "react";
import { LeaderboardEntry, Score } from "src/types";
import { get, getDatabase, ref } from "firebase/database";
import { initializeApp } from "firebase/app";
import { dbroot } from "src/realtimeDatabase";
import { firebaseConfig } from "src/firebaseConfig";

const { publicRuntimeConfig } = getConfig();
const { name } = publicRuntimeConfig.site;
const apiURL =
  "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboard";

const app = initializeApp(firebaseConfig);
const db = getDatabase();

const Home = () => {
  const [level, setLevel] = useState(1);
  const [leaderboardData, setLeaderboardData] = useState({});
  const [child, setChild] = useState<JSX.Element[]>([]);

  const buildLeaderboard = (level: number): JSX.Element[] => {
    const children: JSX.Element[] = [];
    for (const [key, value] of Object.entries(leaderboardData)) {
      const entry = value as LeaderboardEntry;
      for (let s in entry.times) {
        if (s === level.toString()) {
          children.push(
            <div className="flex flex-row justify-between border-b-2 border-gray-200">
              <span>{key}</span>
              <span>{entry.times[s]}</span>
            </div>
          );
        }
      }
    }
    return children;
  };

  useEffect(() => {
    get(ref(db, dbroot)).then((snap) => {
      setLeaderboardData(snap.val());
    });
  }, [level]);

  useEffect(() => {
    const c = buildLeaderboard(level);
    setChild(c);
  }, [leaderboardData]);

  return (
    <Layout>
      <div className="w-full h-screen | flex flex-col items-center | p-2 | bg-slate-900 text-white">
        <h1 className="text-6xl my-10">Freshaliens Leaderboard</h1>
        <div className="flex flex-row">
          <button
            className={
              "p-2 text-yellow-400 border-2 border-current rounded-md" +
              (level === 1 ? " text-black bg-yellow-400" : "")
            }
            type="button"
            onClick={() => setLevel(1)}
          >
            Level 1
          </button>
          <button
            className={
              "p-2 text-yellow-400 border-2 border-current rounded-md" +
              (level === 2 ? " text-black bg-yellow-400" : "")
            }
            type="button"
            onClick={() => setLevel(2)}
          >
            Level 2
          </button>
          <button
            className={
              "p-2 text-yellow-400 border-2 border-current rounded-md" +
              (level === 3 ? " text-black bg-yellow-400" : "")
            }
            type="button"
            onClick={() => setLevel(3)}
          >
            Level 3
          </button>
        </div>
        <div className="w-[50%] mt-5 border-2 p-10">
          <div className="flex flex-row justify-between text-yellow-400 mb-2">
            <span>Name</span>
            <span>Time</span>
          </div>
          {child}
        </div>
      </div>
    </Layout>
  );
};

export default Home;
