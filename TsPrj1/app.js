var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || function (mod) {
    if (mod && mod.__esModule) return mod;
    var result = {};
    if (mod != null) for (var k in mod) if (k !== "default" && Object.prototype.hasOwnProperty.call(mod, k)) __createBinding(result, mod, k);
    __setModuleDefault(result, mod);
    return result;
};
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
Promise.resolve().then(() => __importStar(require('./bscd')));
require('./bscd');
// ִ���첽����
sleepx(5 * 1000);
var f = global["myFunction"];
console.log(f("111"));
//console.log(myFunction("222"))
sleepx(500 * 1000);
//# sourceMappingURL=app.js.map