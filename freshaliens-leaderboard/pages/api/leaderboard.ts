// Next.js API route support: https://nextjs.org/docs/api-routes/introduction
import type { NextApiRequest, NextApiResponse } from "next";
import { Leaderboard, LeaderboardEntry } from "src/types";
import { initializeApp } from "firebase/app";
import { get, getDatabase, ref, set } from "firebase/database";
import { firebaseConfig } from "src/firebaseConfig";
import { dbroot } from "src/realtimeDatabase";

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse
) {
  const app = initializeApp(firebaseConfig);
  const newURL =
    "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboards/official";
  res.status(301).send(newURL);
}
