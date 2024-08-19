import express from 'express';
import path from 'path';
import { echo } from './lib/bsc.js';
echo("echo11111");
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
const port = 3000; // �����Ķ˿�
// ���þ�̬�ļ�Ŀ¼
const publicPath = path.join(__dirname, '');
app.use(express.static(publicPath));
// ������·������
app.get('/', (req, res) => {
    res.sendFile(path.join(publicPath, 'index.html'));
});
console.log("http://localhost:" + port);
app.listen(port, () => {
    console.log(`Server listening on port ${port}`);
});
//# sourceMappingURL=app.js.map