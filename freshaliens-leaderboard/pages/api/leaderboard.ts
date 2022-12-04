// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from "next";
import { Leaderboard, LeaderboardEntry } from "src/types";
import { initializeApp } from "firebase/app";
import { get, getDatabase, ref, set } from "firebase/database";
import { firebaseConfig } from "src/firebaseConfig";
import { dbroot } from "src/realtimeDatabase";

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Leaderboard | string>
) {
  const app = initializeApp(firebaseConfig);
  if (req.method === "GET") {
    const lbref = ref(getDatabase(app), dbroot);
    const data = (await (await get(lbref)).val()) as Leaderboard;
    res.status(200).json(data);
  } else if (req.method === "POST") {
    // curl -d "name=value1&time=value2&level=0" -X POST http://localhost:3000/api/leaderboard
    const name = req.body.name ?? "Anonymous";
    const time = req.body.time ?? "99:99.999";
    const level = req.body.level ?? -1;
    const lberef = ref(getDatabase(app), `${dbroot}/${name}`);
    set(lberef, { time, level });
    res.status(200).json("OK");
  }
}
