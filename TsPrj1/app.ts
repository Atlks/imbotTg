console.log('Hello world');


// ����һ�� sleep ���������ܺ���Ϊ��λ���ӳ�ʱ��
function sleep(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
}

// ʹ�� async ���������� sleep
async function sleepx(ms: number) {
    console.log('Sleeping for 300 seconds...');
    await sleep(ms); // 300000 ���뼴 300 ��
    console.log('Awoke after 300 seconds!');
}

import('./bscd');
require('./bscd');
// ִ���첽����
sleepx(5 * 1000);
var f = global["myFunction"]

console.log(f("111"));
//console.log(myFunction("222"))
sleepx(500 * 1000);