// ����һ������
export function echo(param: string): void {
    console.log(` ${param}`);
}
 
//console.log('Hello world');


// 定义一个 sleep 函数，接受毫秒为单位的延迟时间
export function sleep(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
}

// 使用 async 函数来调用 sleep
//async function sleepx(ms: number) {
//    console.log('Sleeping for 300 seconds...');
//    await sleep(ms); // 300000 毫秒即 300 秒
//    console.log('Awoke after 300 seconds!');
//}
console.log(" load ad bwsc.ts  ")