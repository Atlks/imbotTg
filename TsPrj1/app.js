var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
console.log('Hello world');
// ����һ�� sleep ���������ܺ���Ϊ��λ���ӳ�ʱ��
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
// ʹ�� async ���������� sleep
function sleepx(ms) {
    return __awaiter(this, void 0, void 0, function* () {
        console.log('Sleeping for 300 seconds...');
        yield sleep(ms); // 300000 ���뼴 300 ��
        console.log('Awoke after 300 seconds!');
    });
}
// ִ���첽����
sleepx(300 * 1000);
//# sourceMappingURL=app.js.map