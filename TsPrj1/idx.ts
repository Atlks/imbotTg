
import { sleep,echo } from './lib/bsc.js';


async function main() {

    echo("Statttttt....")
    console.log('Sleeping for 300 seconds...');
    await sleep(5000); // 300000 ���뼴 300 ��
    
    console.log('Awoke after 300 seconds!');
}

main();