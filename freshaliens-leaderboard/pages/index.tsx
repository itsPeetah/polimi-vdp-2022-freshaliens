import getConfig from "next/config";

import Layout from "@/components/Layout";
import { useEffect, useState } from "react";
import { Leaderboard } from "src/types";
import { DataSnapshot, getDatabase, onValue, ref } from "firebase/database";
import { initializeApp } from "firebase/app";
import { dbroot } from "src/realtimeDatabase";
import { firebaseConfig } from "src/firebaseConfig";

const { publicRuntimeConfig } = getConfig();
const { name } = publicRuntimeConfig.site;

const Home = () => {
  const apiURL =
    "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboard";

  const [selectedLevel, setLevel] = useState("1");
  const [leaderboardData, setLeaderboardData] = useState({});
  const [scoreboardEntries, setScoreboardEntries] = useState<JSX.Element>(
    <div>[]</div>
  );

  const buildLeaderboard = (data: Leaderboard) => {
    const children: JSX.Element[] = [];
    for (let name in data) {
      for (let lvl in (data as any)[name]) {
        children.push(
          <ScoreboardEntry
            key={`Leaderboard_${name}_${lvl}`}
            name={name}
            time={(data as any)[name][lvl]}
            show={lvl.toString() === selectedLevel.toString()}
          />
        );
      }
    }

    setScoreboardEntries(<div>{children}</div>);
  };

  useEffect(() => {
    fetch(/*"http://localhost:3000/api/leaderboard"*/ apiURL).then((res) =>
      res.json().then((value) => {
        console.log(value);
        setLeaderboardData(value as Leaderboard);
        buildLeaderboard(value as Leaderboard);
      })
    );
  }, []);

  useEffect(() => {
    buildLeaderboard(leaderboardData as any);
  }, [selectedLevel]);

  return (
    <Layout>
      <div className="w-full h-screen | flex flex-col items-center | p-2 | bg-slate-900 text-white">
        <h1 className="text-6xl my-10">Freshaliens Leaderboard</h1>
        <div className="flex flex-row">
          <LevelButton
            level="1"
            currentLevel={selectedLevel}
            onClick={() => setLevel("1")}
          />
          <LevelButton
            level="2"
            currentLevel={selectedLevel}
            onClick={() => setLevel("2")}
          />
          <LevelButton
            level="3"
            currentLevel={selectedLevel}
            onClick={() => setLevel("3")}
          />
        </div>
        <div className="w-[50%] mt-5 border-2 p-10">
          <div className="flex flex-row justify-between text-yellow-400 mb-2">
            <span>Name</span>
            <span>Time</span>
          </div>
          {scoreboardEntries}
        </div>
      </div>
    </Layout>
  );
};

interface LevelButtonProps {
  level: string;
  currentLevel: string;
  onClick: () => void;
}
const LevelButton = (props: LevelButtonProps) => {
  return (
    <button
      className={
        "p-2 text-yellow-400 border-2 border-current rounded-md" +
        (props.level === props.currentLevel ? " text-black bg-yellow-400" : "")
      }
      type="button"
      onClick={props.onClick}
    >
      Level {props.level}
    </button>
  );
};

interface ScoreboardEntryProps {
  name: string;
  time: string;
  show: boolean;
}
const ScoreboardEntry = (props: ScoreboardEntryProps) => {
  return (
    <div
      className={
        props.show
          ? "flex flex-row justify-between border-b-2 border-gray-200"
          : "hidden"
      }
    >
      <span>{props.name}</span>
      <span>{props.time}</span>
    </div>
  );
};

export default Home;
