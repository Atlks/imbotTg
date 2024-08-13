console.log('Hello world');


// 定义一个 sleep 函数，接受毫秒为单位的延迟时间
function sleep(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
}

// 使用 async 函数来调用 sleep
async function sleepx(ms: number) {
    console.log('Sleeping for 300 seconds...');
    await sleep(ms); // 300000 毫秒即 300 秒
    console.log('Awoke after 300 seconds!');
}

import('./bscd');
require('./bscd');
// 执行异步任务
sleepx(5 * 1000);
var f = global["myFunction"]

console.log(f("111"));
//console.log(myFunction("222"))
sleepx(500 * 1000);