// ����һ������
export function myFunction(param: string): void {
    console.log(`Parameter received: ${param}`);
}
global["myFunction"] = myFunction

console.log(" load ad bwsc.ts  ")