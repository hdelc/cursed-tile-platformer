const fs = require("fs");
const sharp = require("sharp");

async function jsonMapFromImage() {
  const imgBuffer = fs.readFileSync("./input.png");
  const img = await sharp(imgBuffer);

  const width = (await img.metadata()).width;
  const height = (await img.metadata()).height;

  const r = await img.extractChannel("red").raw().toBuffer()
  const g = await img.extractChannel("green").raw().toBuffer()
  const b = await img.extractChannel("blue").raw().toBuffer()
  const a = await img.extractChannel("alpha").raw().toBuffer()

  let arr = Array.from(Array(height), () => new Array(width));
  for (let i = 0; i < r.length; i++) {
    const i1 = i % width;
    const i2 = Math.floor(i / width);
    const rgb = (r[i] + g[i] + g[i]) / 3
    if (a[i] == 0) {
      arr[i2][i1] = 0;
    } else if (rgb == 0) {
      arr[i2][i1] = 1;
    } else if (rgb == 255) {
      arr[i2][i1] = 2;
    }
  }

  console.log(JSON.stringify(arr));
}

async function txtMapFromImage() {
  const imgBuffer = fs.readFileSync("./input.png");
  const img = await sharp(imgBuffer);

  const width = (await img.metadata()).width;
  const height = (await img.metadata()).height;

  const r = await img.extractChannel("red").raw().toBuffer()
  const g = await img.extractChannel("green").raw().toBuffer()
  const b = await img.extractChannel("blue").raw().toBuffer()
  const a = await img.extractChannel("alpha").raw().toBuffer()

  let arr = [];

  for (let i = 0; i < r.length; i++) {
    // const rgb = (r[i] + g[i] + g[i]) / 3
    const rbg = ("#" + parseInt(r.toString(), 16) + parseInt(g.toString(), 16) + parseInt(b.toString(), 16)).toLowerCase();
    if (a[i] == 0) {
      process.stdout.write("0");
    } else if (rgb == "#000000") {
      process.stdout.write("1");
    } else if (rgb == "#ffffff") {
      process.stdout.write("2");
    } else if (rgb == "#00ff00") {
      process.stdout.write("3");
    }
    if ((i+1) % width == 0) {
      process.stdout.write("\n")
    } else {
      process.stdout.write(" ");
    }
  }
}

txtMapFromImage();
