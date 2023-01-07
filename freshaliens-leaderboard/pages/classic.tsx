import getConfig from "next/config";

import Layout from "@/components/Layout";
import { useEffect, useState } from "react";
import { Leaderboard } from "src/types";
import { useRouter } from "next/router";
import Link from "next/link";

const { publicRuntimeConfig } = getConfig();
const { name } = publicRuntimeConfig.site;

const Home = () => {
  const router = useRouter();

  const apiURL =
    "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboards/classic";

  const [selectedLevel, setLevel] = useState("1");
  const [leaderboardData, setLeaderboardData] = useState({});
  const [scoreboardEntries, setScoreboardEntries] = useState<JSX.Element>(
    <div>[]</div>
  );

  const buildLeaderboard = (data: Leaderboard) => {
    const levels: { name: string; time: string; level: string }[] = [];
    const children: JSX.Element[] = [];
    for (let name in data) {
      for (let lvl in (data as any)[name]) {
        if (!!(data as any)[name][lvl] && !!name && lvl !== "0")
          // This is dirty as fuck LMAO
          levels.push({ name, time: (data as any)[name][lvl], level: lvl });
      }
    }

    const sorted = levels.sort((a, b) => {
      if (!!a.time && !!b.time && !!a.time.split && !!b.time.split) {
        const splitA = a.time.split(":");
        const tA = parseFloat(splitA[0]) * 60 + parseFloat(splitA[1]);
        const splitB = b.time.split(":");
        const tB = parseFloat(splitB[0]) * 60 + parseFloat(splitB[1]);
        console.log(tA, tB);
        return tA - tB;
      } else return -1;
    });
    console.log(sorted);
    sorted.forEach((entry) => {
      children.push(
        <ScoreboardEntry
          key={`Leaderboard_${entry.name}_${entry.level}`}
          name={entry.name}
          time={entry.time}
          show={entry.level.toString() === selectedLevel.toString()}
        />
      );
    });

    setScoreboardEntries(<div>{children}</div>);
  };

  useEffect(() => {
    fetch(apiURL).then((res) =>
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
      <div
        className={`w-full h-screen | flex flex-col items-center | p-2 | ${
          !router.query["embed"] ? "bg-slate-900" : "bg-transparent border-2"
        } | text-white | overflow-scroll`}
      >
        {!router.query["embed"] && (
          <div className="text-center">
            <h1 className="text-6xl my-10">
              Freshaliens Leaderboard{" "}
              <span className="text-yellow-400">Classic</span>!
            </h1>
          </div>
        )}
        <Link href="/">
          <a className="group hover:underline my-2">
            &lt; Go to the&nbsp;
            <span className="text-yellow-400 group-hover:text-blue-500">
              official
            </span>
            &nbsp;leaderboard &gt;
          </a>
        </Link>
        <div className="flex flex-row | items-center">
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
        <div className="w-full lg:w-[50%] mt-2 lg:border-2 p-10">
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
