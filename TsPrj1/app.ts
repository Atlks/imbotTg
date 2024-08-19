import express from 'express';
import path from 'path';
import { sleep, echo } from './lib/bsc.js';
echo("echo11111")
//import('./bscd');
//require('./bscd');

//var f = global["myFunction"]

//var s = myFunction("111")
 console.log("start wbsvr...");
//console.log(myFunction("222"))
//sleepx(500 * 1000);

import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const app = express();
const port = 3000; // 监听的端口

// 设置静态文件目录
const publicPath = path.join(__dirname, '');
app.use(express.static(publicPath));

// 处理根路径请求
app.get('/', (req, res) => {
    res.sendFile(path.join(publicPath, 'index.html'));
});

console.log("http://localhost:" + port)
app.listen(port, () => {
    console.log(`Server listening on port ${port}`);
});

